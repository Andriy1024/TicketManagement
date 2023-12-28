using TMS.Common.IntegrationEvents;
using TMS.Common.IntegrationEvents.Notifications;
using TMS.Ticketing.Application.Interfaces;

using TMS.RabbitMq;

namespace TMS.Ticketing.Infrastructure.MessageBroker;

internal sealed class TicketingMessageBrocker : ITicketingMessageBrocker
{
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public TicketingMessageBrocker(IRabbitMqPublisher rabbitMqPublisher)
    {
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    public void Send(OrderStatusUpdatedNotification integrationEvent)
    {
        // TODO: Use transactional outbox pattern
        _rabbitMqPublisher.Publish(new IntegrationEvent<OrderStatusUpdatedNotification>
        {
            Payload = integrationEvent
        },
        p =>
        {
            p.Exchange.Name = Exchange.Name.Notifications;
            p.Exchange.Type = Exchange.Type.Direct;
            p.RoutingKey = typeof(OrderStatusUpdatedNotification).Name;
            p.EnableRetryPolicy = true;
        });
    }
}
