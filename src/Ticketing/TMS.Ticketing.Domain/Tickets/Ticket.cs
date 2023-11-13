namespace TMS.Ticketing.Domain.Tickets;

public enum TicketStatus
{
    Pending = 1,
    CheckedIn = 2,
    Canceled = 3
}

public sealed class Ticket : IDocumentEntry<Guid>
{
    public static string Collection => "Tickets";

    public required Guid Id { get; init; }

    public required Guid EventId { get; init; }

    public required Guid OrderId { get; init; }

    public required Guid SeatId { get; init; }

    public required Guid PriceId { get; init; }

    public required TicketStatus Status { get; set; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; set; }

    public required string ValidationHashCode { get; init; }
}