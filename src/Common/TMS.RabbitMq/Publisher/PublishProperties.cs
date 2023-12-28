namespace TMS.RabbitMq;

public record PublishProperties 
{
    public ExchangeProperties Exchange { get; } = new();

    public string RoutingKey { get; set; } = "";

    public bool EnableRetryPolicy { get; set; }
}