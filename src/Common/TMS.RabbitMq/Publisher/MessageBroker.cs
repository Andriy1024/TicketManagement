namespace TMS.RabbitMq.Publisher;

public class MessageBroker : IMessageBroker
{
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public MessageBroker(IRabbitMqPublisher rabbitMqPublisher)
    {
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    public void Publish<T>(T integrationEvent, PublishProperties properties) where T : IIntegrationEvent
    {
        _rabbitMqPublisher.Publish(integrationEvent, properties);
    }

    public void Publish<T>(T integrationEvent, Action<PublishProperties> action) where T : IIntegrationEvent
    {
        var properties = new PublishProperties();

        action(properties);

        Publish(integrationEvent, properties);
    }
}