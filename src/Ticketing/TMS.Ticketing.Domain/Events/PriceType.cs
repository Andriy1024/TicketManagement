namespace TMS.Ticketing.Domain.Events;

public sealed class PriceType
{
    public required int EventId { get; set; }

    public required int Id { get; set; }

    public required decimal Amount { get; set; }

    public required string Currency { get; set; }

    public required string Name { get; set; }
}