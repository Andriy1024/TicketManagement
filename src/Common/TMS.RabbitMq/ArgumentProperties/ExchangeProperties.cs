namespace TMS.RabbitMq;

public record ExchangeProperties 
{
    public string Name { get; set; }

    public string Type { get; set; } = Exchange.Type.Direct;

    public bool AutoDelete { get; set; } = true;

    public bool Durable { get; set; } = false;

    public bool IsFanout => Type == Exchange.Type.Fanout;
}