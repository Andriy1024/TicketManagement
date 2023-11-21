using TMS.Common.Enums;
using TMS.Payments.Domain.Abstractions;
using TMS.Payments.Domain.DomainEvents;
using TMS.Payments.Domain.Enums;

namespace TMS.Payments.Domain.Entities;

public sealed class PaymentAggregate : EventSourcedAggregate
{
    public override string GetId() => PaymentId.ToString();

    public Guid PaymentId { get; private set; }

    public int AccountId { get; private set; }

    public PaymentType Type { get; private set; }

    public PaymentStatus Status { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime Created { get; private set; }

    public DateTime Updated { get; private set; }

    public static PaymentAggregate Create(
        Guid id,  
        decimal amount,
        int accountId,
        PaymentType type)
    {
        var aggregate = new PaymentAggregate();

        var @event = new PaymentCreatedEvent()
        {
            PaymentId = id,
            AccountId = accountId,
            Type = type,
            Amount = amount,
            Status = PaymentStatus.Pending,
            Message = "Payment created.",
            Created = DateTime.UtcNow,
        };

        aggregate.Apply(@event);

        return aggregate;
    }

    public static PaymentAggregate Load(List<IPaymentEvent> events)
    {
        var aggregate = new PaymentAggregate();

        aggregate.Load(events.Cast<IDomainEvent>());

        return aggregate;
    }

    public void Completed()
    {
        var @event = new PaymentStatusUpdated()
        {
            PaymentId = PaymentId,
            AccountId = AccountId,
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
            AccountId = AccountId,
            Status = PaymentStatus.Failed,
            CreateAt = DateTime.UtcNow,
            Message = "Payment failed"
        };

        Apply(@event);
    }

    protected override void When(IDomainEvent @event)
    {
        switch (@event)
        {
            case PaymentCreatedEvent e:
                When(e);
                break;
            case PaymentStatusUpdated e:
                When(e);
                break;
            default:
                throw new NotImplementedException($"Unknown event type: {@event.GetType().Name}.");
        }
    }

    private void When(PaymentCreatedEvent @event)
    {
        PaymentId = @event.PaymentId;
        AccountId = @event.AccountId;
        Type = @event.Type;
        Status = @event.Status;
        Amount = @event.Amount;
        Created = @event.Created;
        Updated = @event.Created;
    }

    private void When(PaymentStatusUpdated @event)
    {
        Status = @event.Status;
        Updated = @event.CreateAt;
    }
}