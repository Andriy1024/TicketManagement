namespace TMS.Common.Interfaces;

public interface IEventDrivenEntity
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void AddDomainEvent(IDomainEvent eventItem);

    IReadOnlyCollection<IDomainEvent> ExtractDomainEvents();
}

public interface IEntity<out TKey>
{
    TKey Id { get; }

    public int Version { get; }

    public (int Old, int New) IncreaseVersion();
}

public interface IEventDrivenEntity<out TKey> : IEventDrivenEntity, IEntity<TKey>
{
}