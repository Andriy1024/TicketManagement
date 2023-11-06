namespace TMS.Ticketing.Domain.Events;

public sealed class EventManager
{
    public required int AccountId { get; init; }

    public required int EventId { get; init; }
}