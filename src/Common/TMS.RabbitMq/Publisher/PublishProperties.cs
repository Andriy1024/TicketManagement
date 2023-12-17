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

    public static PublishProperties Validate(PublishProperties properties, bool allowEmptyRoutingKey = false) 
    {
        if (!allowEmptyRoutingKey) 
        {
            ArgumentNullException.ThrowIfNullOrEmpty(properties.RoutingKey);
        }
            
        return properties;
    }

    public static string CreateRoutingKey<T>(T integrationEvent)
        where T : IIntegrationEvent
    { 
        return integrationEvent.GetType().Name;
    }
}
