namespace TMS.RabbitMq;

public record SubscribeProperties
{
    public ExchangeProperties Exchange { get; } = new ExchangeProperties();

    public QueueProperties Queue { get; } = new QueueProperties();

    public ConsumerProperties Consumer { get; } = new ConsumerProperties();

    public override string ToString()
    {
        return $"{nameof(Exchange)}: {Exchange}, {nameof(Queue)}: {Queue}, {nameof(Consumer)}: {Consumer}.";
    }
}
