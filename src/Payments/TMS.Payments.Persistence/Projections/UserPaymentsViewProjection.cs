using Marten.Events.Projections;

using TMS.Payments.Domain.DomainEvents;
using TMS.Payments.Domain.Views;

namespace TMS.Payments.Persistence.Projections;

public class UserPaymentsViewProjection : MultiStreamProjection<UserPaymentsView, int>
{
    public UserPaymentsViewProjection()
    {
        Identity<PaymentCreatedEvent>((ev) => ev.AccountId);
        Identity<PaymentStatusUpdated>((ev) => ev.AccountId);

        ProjectEvent<PaymentCreatedEvent>((view, @event) => view.Apply(@event));
        ProjectEvent<PaymentStatusUpdated>((view, @event) => view.Apply(@event));
    }
}