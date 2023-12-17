namespace TMS.RabbitMq.Publisher;

public interface IMessageBroker
{
    void Publish<T>(T integrationEvent, PublishProperties properties) where T : IIntegrationEvent;

    void Publish<T>(T integrationEvent, Action<PublishProperties> action) where T : IIntegrationEvent;
}