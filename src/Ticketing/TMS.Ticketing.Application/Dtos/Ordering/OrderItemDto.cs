using TMS.Ticketing.Domain.Ordering;

namespace TMS.Ticketing.Application.Dtos;

public class OrderItemDto
{
    public required Guid EventId { get; init; }

    public required Guid SeatId { get; init; }

    public required Guid PriceId { get; init; }

    public required decimal Amount { get; init; }

    public static OrderItemDto Map(OrderItem item) => new()
    {
        EventId = item.EventId,
        SeatId = item.SeatId,
        PriceId = item.PriceId,
        Amount = item.Amount,
    };
}
