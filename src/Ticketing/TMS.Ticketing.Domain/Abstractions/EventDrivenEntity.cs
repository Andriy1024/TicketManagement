using TMS.Common.Interfaces;

namespace TMS.Ticketing.Domain;

public abstract class EventDrivenEntity : IEventDrivenEntity
{
    private static readonly IReadOnlyCollection<IDomainEvent> _emptyEvents = new List<IDomainEvent>(0);

    private List<IDomainEvent>? _domainEvents;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly() ?? _emptyEvents;

    public virtual void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents ??= new List<IDomainEvent>();

        _domainEvents.Add(eventItem);
    }

    public IReadOnlyCollection<IDomainEvent> ExtractDomainEvents()
    {
        var result = _domainEvents?.ToList() ?? _emptyEvents;

        _domainEvents?.Clear();

        return result;
    }
}

public abstract class EventDrivenEntity<TKey> : EventDrivenEntity, IEventDrivenEntity<TKey>
{
    public required TKey Id { get; init; }

    public int Version { get; set; } = 1;

    public (int Old, int New) IncreaseVersion() => (Version, ++Version);
}