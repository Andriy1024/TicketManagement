namespace TMS.Ticketing.Domain.Events;

public sealed class Price
{
    public required Guid Id { get; set; }

    public required Guid EventId { get; set; }

    public required decimal Amount { get; set; }

    public required string Currency { get; set; }

    public required string Name { get; set; }
}