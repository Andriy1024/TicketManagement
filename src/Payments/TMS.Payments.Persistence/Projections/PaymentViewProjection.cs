using Marten.Events.Projections;

using TMS.Payments.Domain.DomainEvents;
using TMS.Payments.Domain.Views;

namespace TMS.Payments.Persistence.Projections;

public class PaymentViewProjection : MultiStreamProjection<PaymentDetailsView, Guid>
{
    public PaymentViewProjection()
    {
        Identity<PaymentCreatedEvent>((ev) => ev.PaymentId);
        Identity<PaymentStatusUpdated>((ev) => ev.PaymentId);

        ProjectEvent<PaymentCreatedEvent>((view, @event) => view.Apply(@event));
        ProjectEvent<PaymentStatusUpdated>((view, @event) => view.Apply(@event));
    }
}