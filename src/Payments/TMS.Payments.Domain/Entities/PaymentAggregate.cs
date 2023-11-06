using TMS.Payments.Domain.DomainEvents;
using TMS.Payments.Domain.Enums;
using TMS.Payments.Domain.Interfaces;

namespace TMS.Payments.Domain.Entities;

public sealed class PaymentAggregate
{
    public Guid PaymentId { get; private set; }

    public PaymentType Type { get; private set; }

    public PaymentStatus Status { get; private set; }

    public DateTime Created { get; private set; }

    public DateTime Updated { get; private set; }

    /// <summary>
    /// The Changes that should be committed to an event store.
    /// </summary>
    public IReadOnlyCollection<IPaymentEvent> Changes => _changes;

    private readonly List<IPaymentEvent> _changes = new();

    /// <summary>
    /// The Paymen's event history.
    /// </summary>
    public IReadOnlyCollection<IPaymentEvent> History => _history;

    private readonly List<IPaymentEvent> _history = new();

    /// <summary>
    /// Version represents the number of commits events, and used to handle concurrency.
    /// </summary>
    public int Version { get; private set; }

    public static PaymentAggregate Create(Guid id, PaymentType type)
    {
        var aggregate = new PaymentAggregate();

        var @event = new PaymentCreatedEvent()
        {
            PaymentId = id,
            Type = type,
            Created = DateTime.UtcNow,
            Status = PaymentStatus.Pending,
            Message = "Payment created."
        };

        aggregate.Apply(@event);

        return aggregate;
    }

    public static PaymentAggregate Load(List<IPaymentEvent> events)
    {
        var aggregate = new PaymentAggregate();

        foreach (var @event in events)
        {
            aggregate.When((dynamic)@event);
            aggregate._history.Add(@event);
            aggregate.Version++;
        }

        return aggregate;
    }

    private void Apply(IPaymentEvent evt)
    {
        When((dynamic)evt);
        _changes.Add(evt);
        _history.Add(evt);
        Version++;
    }

    public void Completed()
    {
        var @event = new PaymentStatusUpdated()
        {
            PaymentId = PaymentId,
            Status = PaymentStatus.Completed,
            CreateAt = DateTime.UtcNow,
            Message = "Payment completed"
        };

        Apply(@event);
    }

    public void Failed()
    {
        var @event = new PaymentStatusUpdated()
        {
            PaymentId = PaymentId,
            Status = PaymentStatus.Failed,
            CreateAt = DateTime.UtcNow,
            Message = "Payment failed"
        };

        Apply(@event);
    }

    private void When(PaymentCreatedEvent @event)
    {
        PaymentId = @event.PaymentId;
        Type = @event.Type;
        Status = @event.Status;
        Created = @event.Created;
        Updated = @event.Created;
    }

    private void When(PaymentStatusUpdated @event)
    {
        Status = @event.Status;
        Updated = @event.CreateAt;
    }
}