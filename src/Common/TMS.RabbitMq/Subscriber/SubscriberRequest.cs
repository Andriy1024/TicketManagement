namespace TMS.RabbitMq.Pipeline;

public record SubscriberRequest(
    BasicDeliverEventArgs RabbitPrperties, 
    IIntegrationEvent IntegrationEvent);