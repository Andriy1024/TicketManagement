namespace TMS.Payments.Domain.Abstractions;

public abstract class EventSourcedAggregate : IEventSourcedAggregate
{
    /// <summary>
    /// The Changes that should be committed to an event store.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> Changes => _changes;

    private readonly List<IDomainEvent> _changes = new List<IDomainEvent>();

    /// <summary>
    /// The full aggregate's event history.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> History => _history;

    private readonly List<IDomainEvent> _history = new();

    /// <summary>
    /// Version represents the number of commits events.
    /// </summary>
    public int Version { get; private set; }

    public abstract string GetId();

    protected abstract void When(IDomainEvent @event);

    /// <summary>
    /// The action should be triggered after changes committed to an event store.
    /// </summary>
    public void ClearChanges() => _changes.Clear();

    protected void Apply(IDomainEvent @event)
    {
        _changes.Add(@event);
        _history.Add(@event);
        When(@event);
        Version++;
    }

    public void Load(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            When(@event);
            _history.Add(@event);
            Version++;
        }
    }
}