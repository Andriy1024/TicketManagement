using TMS.Payments.Domain.Enums;
using TMS.Payments.Domain.Interfaces;

namespace TMS.Payments.Domain.DomainEvents;

public sealed class PaymentStatusUpdated : IPaymentEvent
{
    public required Guid PaymentId { get; init; }

    public required PaymentStatus Status { get; init; }

    public required DateTime CreateAt { get; init; }

    public required string Message { get; init; }
}