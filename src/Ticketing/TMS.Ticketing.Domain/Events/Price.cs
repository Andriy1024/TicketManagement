namespace TMS.Ticketing.Domain.Events;

public sealed class Price
{
    public required int EventId { get; set; }

    public required int Id { get; set; }

    public required decimal Amount { get; set; }

    public required string Currency { get; set; }

    public required string Name { get; set; }
}