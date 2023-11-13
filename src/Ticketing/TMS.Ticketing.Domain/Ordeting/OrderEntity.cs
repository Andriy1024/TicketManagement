using TMS.Ticketing.Domain.Ordeting;

namespace TMS.Ticketing.Domain.Ordering;

public sealed class OrderEntity : IEntity<Guid>
{
    public Guid Id { get; init; }

    public Guid EventId { get; init; }

    public int AccountId { get; init; }

    public OrderStatus Status { get; set; }

    public int PaymentId { get; init; }

    public decimal Total => this.OrderItems.Sum(x => x.Amount);

    public required List<OrderItem> OrderItems { get; init; } = new();

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }
}