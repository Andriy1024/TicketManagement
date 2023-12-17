namespace TMS.RabbitMq;

public interface IRabbitMqPublisher
{
    void Publish<T>(T integrationEvent, PublishProperties properties)
           where T : IIntegrationEvent;

    void Publish<T>(T integrationEvent, Action<PublishProperties> properties)
           where T : IIntegrationEvent;

    void AddPublisher(PublishProperties properties);

    bool RemovePublisher(PublishProperties properties);
}
