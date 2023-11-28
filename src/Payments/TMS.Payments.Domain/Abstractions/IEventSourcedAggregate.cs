namespace TMS.Payments.Domain.Abstractions;

public interface IEventSourcedAggregate
{
    string GetId();

    int Version { get; }

    IReadOnlyCollection<IDomainEvent> Changes { get; }

    void ClearChanges();

    void Load(IEnumerable<IDomainEvent> events);
}