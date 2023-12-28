using TMS.Common.IntegrationEvents;

using TMS.Payments.Application.Interfaces;

using TMS.RabbitMq;

namespace TMS.Payments.Infrastructure.MessageBrocker;

public sealed class PaymentsMessageBrocker : IPaymentsMessageBrocker
{
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public PaymentsMessageBrocker(IRabbitMqPublisher rabbitMqPublisher)
    {
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    public Task SendAsync(PaymentStatusUpdated @event)
    {
        // TODO: Use transactional outbox pattern

        _rabbitMqPublisher.Publish(new IntegrationEvent<PaymentStatusUpdated>
        {
            Payload = @event
        }, 
        p =>
        {
            p.Exchange.Name = Exchange.Name.Payments;
            p.Exchange.Type = Exchange.Type.Direct;
            p.RoutingKey = typeof(PaymentStatusUpdated).Name;
            p.EnableRetryPolicy = true;
        });

        return Task.CompletedTask;
    }
}
