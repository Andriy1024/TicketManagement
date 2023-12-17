namespace TMS.RabbitMq;

public record ExchangeProperties 
{
    public string Name { get; set; }

    public string Type { get; set; }

    public bool AutoDelete { get; set; } = true;

    public bool Durable { get; set; } = false;

    public bool IsFanout => Type == Exchange.Type.Fanout;

    public ExchangeProperties() {}

    public ExchangeProperties(string exchangeName)
    {
        Name = exchangeName;
    }

    public ExchangeProperties(
        string exchangeName, string exchangeType, 
        bool autoDelete, bool durable) 
        : this(exchangeName)
    {
        Type = exchangeType;
        AutoDelete = autoDelete;
        Durable = durable;
    }
}