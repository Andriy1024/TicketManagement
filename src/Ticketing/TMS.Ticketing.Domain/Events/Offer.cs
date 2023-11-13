namespace TMS.Ticketing.Domain.Events;

public sealed class Offer
{
    public required Guid SeatId { get; set; }

    public required Guid PriceId { get; set; }
}