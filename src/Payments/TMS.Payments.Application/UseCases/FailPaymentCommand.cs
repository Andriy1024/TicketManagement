using MediatR;

using TMS.Common.Errors;
using TMS.Payments.Application.Interfaces;
using TMS.Payments.Domain.Entities;
using TMS.Payments.Domain.Enums;

namespace TMS.Payments.Application.UseCases;

public sealed record FailPaymentCommand(Guid PaymentId) : IRequest<FailPaymentResult>;

public sealed record FailPaymentResult(Guid PaymentId, PaymentStatus Status);

public sealed class FailPaymentHandler : IRequestHandler<FailPaymentCommand, FailPaymentResult>
{
    private readonly IPaymentsEventStore _eventStore;

    public FailPaymentHandler(IPaymentsEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<FailPaymentResult> Handle(FailPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _eventStore.LoadAsync<PaymentAggregate>(request.PaymentId.ToString());

        if (payment == null)
        {
            throw AppError.NotFound("Payment not found").ToException();
        }

        payment.Failed();

        await _eventStore.StoreAsync(payment);

        return new FailPaymentResult(payment.PaymentId, payment.Status);
    }
}
