using TMS.Common.Interfaces;

namespace TMS.Ticketing.Domain;

public interface IEntity
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void AddDomainEvent(IDomainEvent eventItem);

    IReadOnlyCollection<IDomainEvent> ExtractDomainEvents();
}

public interface IEntity<out TKey> : IEntity
{
    TKey Id { get; }

    public int Version { get; }

    public (int Old, int New) IncreaseVersion();
}