using TMS.Common.IntegrationEvents.Notifications;

namespace TMS.Ticketing.Application.Interfaces;

public interface ITicketingMessageBrocker
{
    void Send(OrderStatusUpdatedNotification integrationEvent);
}
