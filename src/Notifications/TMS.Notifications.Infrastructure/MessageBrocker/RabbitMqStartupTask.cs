using TMS.Common.IntegrationEvents.Notifications;
using TMS.Common.IntegrationEvents;
using TMS.Common.Interfaces;

using TMS.RabbitMq;

namespace TMS.Notifications.Infrastructure.MessageBrocker;

public class RabbitMqStartupTask : IStartupTask
{
    private readonly IRabbitMqSubscriber _messageBus;

    public RabbitMqStartupTask(IRabbitMqSubscriber messageBus)
    {
        _messageBus = messageBus;
    }

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _messageBus.Subscribe<IntegrationEvent<OrderStatusUpdatedNotification>>(o =>
        {
            o.Exchange.Name = Exchange.Name.Notifications;
            o.Exchange.Type = Exchange.Type.Direct;
            o.Queue.RoutingKey = typeof(OrderStatusUpdatedNotification).Name;
            o.Queue.AutoDelete = false;
            o.Queue.Exclusive = false;
            o.Consumer.Autoack = false;
        });

        return Task.CompletedTask;
    }
}