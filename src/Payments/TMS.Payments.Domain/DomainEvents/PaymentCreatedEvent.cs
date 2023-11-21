using TMS.Common.Enums;

using TMS.Payments.Domain.Abstractions;
using TMS.Payments.Domain.Enums;

namespace TMS.Payments.Domain.DomainEvents;

public sealed class PaymentCreatedEvent : IPaymentEvent
{
    public required Guid PaymentId { get; init; }

    public required int AccountId { get; init; }

    public required PaymentType Type { get; init; }

    public required PaymentStatus Status { get; init; }

    public required decimal Amount { get; init; }

    public required string Message { get; init; }

    public required DateTime Created { get; init; }
}
