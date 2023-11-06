using TMS.Ticketing.Domain.Tickets;

namespace TMS.Ticketing.Domain.Ordering;

public sealed class Order
{
    public int OrderId { get; init; }

    public int AccountId { get; init; }

    public int EventId { get; init; }

    public decimal Total => this.Items.Sum(x => x.Amount);
    
    public required List<TicketItem> Items { get; init; }

    public OrderStatus Status { get; init; }

    public int PaymentId { get; init; }

    public int? RefundId { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}