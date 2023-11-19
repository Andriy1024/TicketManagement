using TMS.Payments.Domain.Abstractions;
using TMS.Payments.Domain.Enums;

namespace TMS.Payments.Domain.DomainEvents;

public sealed class PaymentStatusUpdated : IPaymentEvent
{
    public required Guid PaymentId { get; init; }

    public required int AccountId { get; init; }

    public required PaymentStatus Status { get; init; }

    public required DateTime CreateAt { get; init; }

    public required string Message { get; init; }
}