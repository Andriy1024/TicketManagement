namespace TMS.Ticketing.Domain.Tickets;

public sealed class Ticket
{
    public required int TicketId { get; set; }

    public required int EventId { get; set; }

    public required TicketStatus Status { get; set; }

    public required TicketItem[] Seats { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required DateTime UpdatedAt { get; set; }

    public required string ValidationHashCode { get; set; }
}