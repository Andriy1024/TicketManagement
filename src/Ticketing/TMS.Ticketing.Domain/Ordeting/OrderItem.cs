namespace TMS.Ticketing.Domain.Ordering;

public sealed class OrderItem
{
    public required Guid EventId { get; init; }

    public required Guid SeatId { get; init; }

    public required Guid PriceId { get; init; }

    public required decimal Amount { get; init; }
}