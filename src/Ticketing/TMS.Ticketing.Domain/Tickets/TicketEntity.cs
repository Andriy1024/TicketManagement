namespace TMS.Ticketing.Domain.Tickets;

public sealed class TicketEntity : Entity<Guid>
{
    public required Guid EventId { get; init; }

    public required Guid OrderId { get; init; }

    public required Guid SeatId { get; init; }

    public required Guid PriceId { get; init; }

    public required TicketStatus Status { get; set; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; set; }

    public required string ValidationHashCode { get; init; }
}