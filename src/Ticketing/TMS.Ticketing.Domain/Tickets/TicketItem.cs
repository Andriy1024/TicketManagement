namespace TMS.Ticketing.Domain.Tickets;

public sealed class TicketItem
{
    public required int SeatId { get; set; }

    public required int PriceId { get; set; }

    public required string Currency { get; set; }

    public required decimal Amount { get; set; }
}