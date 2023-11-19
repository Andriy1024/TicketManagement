using TMS.Payments.Domain.DomainEvents;
using TMS.Payments.Domain.Enums;

namespace TMS.Payments.Domain.Views;

public class PaymentHistoryRecord 
{
    public PaymentStatus Status { get; set; }

    public string Message { get; set; }

    public DateTime CreateAt { get; set; }
}

public sealed class PaymentDetailsView : PaymentOverview
{
    public PaymentType Type { get; set; }

    public decimal Amount { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public List<PaymentHistoryRecord> History { get; set; } = new();

    public void Apply(PaymentCreatedEvent @event) 
    {
        PaymentId = @event.PaymentId;
        AccountId = @event.AccountId;
        Type = @event.Type;
        Status = @event.Status;
        Amount = @event.Amount;
        Created = @event.Created;
        Updated = @event.Created;
        History.Add(new PaymentHistoryRecord 
        {
            Status = @event.Status,
            Message = @event.Message,
            CreateAt = @event.Created
        });
    }

    public void Apply(PaymentStatusUpdated @event)
    {
        Status = @event.Status;
        Updated = @event.CreateAt;
        History.Add(new PaymentHistoryRecord
        {
            Status = @event.Status,
            Message = @event.Message,
            CreateAt = @event.CreateAt
        });
    }
}