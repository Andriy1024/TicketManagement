using TMS.Payments.Domain.DomainEvents;

namespace TMS.Payments.Domain.Views;

public sealed class UserPaymentsView 
{
    public int AccountId { get; set; }

    public List<PaymentOverview> Payments { get; set; } = new();

    public void Apply(PaymentCreatedEvent @event)
    {
        AccountId = @event.AccountId;

        Payments.Add(new PaymentOverview
        {
            PaymentId = @event.PaymentId,
            AccountId = @event.AccountId,
            Status = @event.Status,
        });
    }

    public void Apply(PaymentStatusUpdated @event)
    {
        
        var payment = Payments.First(x => x.PaymentId == @event.PaymentId);

        payment.Status = @event.Status;
    }
}