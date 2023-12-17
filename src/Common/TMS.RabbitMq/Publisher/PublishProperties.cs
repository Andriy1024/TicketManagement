namespace TMS.RabbitMq;

public record PublishProperties 
{
    public ExchangeProperties Exchange { get; }

    public string RoutingKey { get; set; } = "";

    public bool EnableRetryPolicy { get; set; }

    public PublishProperties()
        : this(new ExchangeProperties())
    {
    }

    public PublishProperties(string exchangeName)
        : this(new ExchangeProperties(exchangeName))
    {
    }

    public PublishProperties(ExchangeProperties exchange)
    {
        Exchange = exchange;
    }

    public static string CreateRoutingKey<T>(T integrationEvent)
        where T : IIntegrationEvent
    { 
        return integrationEvent.GetType().Name;
    }
}
