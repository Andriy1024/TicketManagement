using MediatR;

using TMS.Common.Errors;
using TMS.Payments.Application.Interfaces;
using TMS.Payments.Domain.Entities;
using TMS.Payments.Domain.Enums;

namespace TMS.Payments.Application.UseCases;

public sealed record CompletePaymentCommand(Guid PaymentId) : IRequest<CompletePaymentResult>;

public sealed record CompletePaymentResult(Guid PaymentId, PaymentStatus Status);

public sealed class CompletePaymentHandler : IRequestHandler<CompletePaymentCommand, CompletePaymentResult>
{
    private readonly IPaymentsEventStore _eventStore;

    public CompletePaymentHandler(IPaymentsEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<CompletePaymentResult> Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _eventStore.LoadAsync<PaymentAggregate>(request.PaymentId.ToString());

        if (payment == null)
        {
            throw AppError.NotFound("Payment not found").ToException();
        }

        payment.Completed();

        await _eventStore.StoreAsync(payment);

        return new CompletePaymentResult(payment.PaymentId, payment.Status);
    }
}