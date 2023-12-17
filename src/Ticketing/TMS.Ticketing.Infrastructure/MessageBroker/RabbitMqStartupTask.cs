using TMS.Common.IntegrationEvents;
using TMS.Common.Interfaces;

using TMS.RabbitMq;

namespace TMS.Ticketing.Infrastructure.MessageBroker;

internal class RabbitMqStartupTask : IStartupTask
{
    private readonly IRabbitMqSubscriber _messageBus;

    public RabbitMqStartupTask(IRabbitMqSubscriber messageBus)
    {
        _messageBus = messageBus;
    }

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _messageBus.Subscribe<IntegrationEvent<PaymentStatusUpdated>>(o =>
        {
            o.Exchange.Name = Exchange.Name.Payments;
            o.Exchange.Type = Exchange.Type.Direct;
            o.Queue.AutoDelete = false;
            o.Queue.Exclusive = false;
            o.Consumer.Autoack = false;
        });

        return Task.CompletedTask;
    }
}
