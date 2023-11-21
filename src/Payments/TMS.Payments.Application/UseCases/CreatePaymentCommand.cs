using MediatR;
using TMS.Common.Enums;
using TMS.Payments.Application.Interfaces;
using TMS.Payments.Domain.Entities;
using TMS.Payments.Domain.Enums;

namespace TMS.Payments.Application.UseCases;

public sealed class CreatePaymentCommand : IRequest<CreatePaymentResult>
{
    public Guid PaymentId { get; set; }

    // Will be taken from jwt token in future.
    public int AccountId { get; set; }

    public decimal Amount { get; set; }
}

public sealed record CreatePaymentResult(Guid PaymentId, PaymentStatus Status);

internal sealed class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResult>
{
    private readonly IPaymentsEventStore _eventStore;

    public CreatePaymentHandler(IPaymentsEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<CreatePaymentResult> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = PaymentAggregate.Create(
            request.PaymentId, 
            request.Amount, 
            request.AccountId,
            PaymentType.Payment);

        await _eventStore.StoreAsync(payment);

        return new CreatePaymentResult(payment.PaymentId, payment.Status);
    }
}