using TMS.Common.Enums;
using TMS.Common.IntegrationEvents;
using TMS.Payments.Application.Interfaces;
using TMS.Payments.Application.MessageBrocker;
using TMS.Payments.Domain.Entities;

namespace TMS.Payments.Application.UseCases;

public sealed record CompletePaymentCommand(Guid PaymentId) : IRequest<CompletePaymentResult>, IValidatable 
{
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x =>
            x.RuleFor(y => y.PaymentId).NotEmpty());
    }
};

public sealed record CompletePaymentResult(Guid PaymentId, PaymentStatus Status);

public sealed class CompletePaymentHandler : IRequestHandler<CompletePaymentCommand, CompletePaymentResult>
{
    private readonly IPaymentsEventStore _eventStore;
    private readonly IMessageBrocker _messageBrocker;

    public CompletePaymentHandler(IPaymentsEventStore eventStore, IMessageBrocker messageBrocker)
    {
        _eventStore = eventStore;
        _messageBrocker = messageBrocker;
    }

    public async Task<CompletePaymentResult> Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _eventStore.LoadAsync<PaymentAggregate>(request.PaymentId.ToString());

        if (payment == null)
        {
            throw ApiError.NotFound("Payment not found").ToException();
        }

        payment.Completed();

        await _eventStore.StoreAsync(payment);

        await _messageBrocker.SendAsync(new PaymentStatusUpdated 
        {
            PaymentId = payment.PaymentId,
            Status = payment.Status,
        });

        return new CompletePaymentResult(payment.PaymentId, payment.Status);
    }
}