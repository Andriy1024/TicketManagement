using TMS.Ticketing.Domain.Tickets;

namespace TMS.Ticketing.Domain.Ordering;

public sealed class Cart
{
    public required Guid CartId { get; init; }

    public decimal Total => this.Items.Sum(x => x.Amount);

    public required List<TicketItem> Items { get; init; } = new();
}