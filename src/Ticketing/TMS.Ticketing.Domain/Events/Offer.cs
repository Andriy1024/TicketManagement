namespace TMS.Ticketing.Domain.Events;

public sealed class Offer
{
    public required int SeatId { get; set; }

    public required int PriceId { get; set; }
}