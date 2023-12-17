using Refit;

using TMS.Common.IntegrationEvents;

namespace TMS.Payments.Infrastructure.Ticketing;

public interface ITicketingApi
{
    [Post("/api/webhooks/payments/status")]
    Task PaymentStatusUpdatedAsync([Body] IntegrationEvent<PaymentStatusUpdated> @event);
}
