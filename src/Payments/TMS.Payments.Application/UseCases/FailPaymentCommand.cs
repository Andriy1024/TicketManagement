using TMS.Common.Enums;
using TMS.Common.IntegrationEvents;
using TMS.Payments.Application.Interfaces;
using TMS.Payments.Domain.Entities;

namespace TMS.Payments.Application.UseCases;

public sealed record FailPaymentCommand(Guid PaymentId) : IRequest<FailPaymentResult>, IValidatable 
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
            x.RuleFor(y => y.PaymentId).NotEmpty());
    }
};

public sealed record FailPaymentResult(Guid PaymentId, PaymentStatus Status);

public sealed class FailPaymentHandler : IRequestHandler<FailPaymentCommand, FailPaymentResult>
{
    private readonly IPaymentsEventStore _eventStore;
    private readonly IPaymentsMessageBrocker _messageBrocker;

    public FailPaymentHandler(IPaymentsEventStore eventStore, IPaymentsMessageBrocker messageBrocker)
    {
        _eventStore = eventStore;
        _messageBrocker = messageBrocker;
    }

    public async Task<FailPaymentResult> Handle(FailPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _eventStore.LoadAsync<PaymentAggregate>(request.PaymentId.ToString());

        if (payment == null)
        {
            throw ApiError.NotFound("Payment not found").ToException();
        }

        payment.Failed();

        await _eventStore.StoreAsync(payment);

        await _messageBrocker.SendAsync(new PaymentStatusUpdated
        {
            PaymentId = payment.PaymentId,
            Status = payment.Status,
        });

        return new FailPaymentResult(payment.PaymentId, payment.Status);
    }
}
