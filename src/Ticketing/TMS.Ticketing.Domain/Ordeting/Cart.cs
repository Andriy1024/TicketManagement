using TMS.Ticketing.Domain.Ordeting;

namespace TMS.Ticketing.Domain.Ordering;

public sealed class Cart : IDocumentEntry<Guid>
{
    public static string Collection => "Carts";

    public required Guid Id { get; init; }

    public int AccountId { get; init; }

    public decimal Total => this.OrderItems.Sum(x => x.Amount);

    public List<OrderItem> OrderItems { get; } = new();
}