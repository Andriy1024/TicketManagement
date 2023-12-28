namespace TMS.RabbitMq;

internal sealed record SubscriberContext(
    SubscriptionKey Key, 
    Type EventType, 
    IModel Channel, 
    QueueProperties Queue, 
    ExchangeProperties Exchange, 
    ConsumerProperties Consumer)
{
    public override string ToString()
    {
        return $"{nameof(Queue)}: {Queue}, {nameof(Exchange)}: {Exchange}, {nameof(Consumer)}: {Consumer}, {nameof(EventType)}: {EventType.Name}";
    }
}