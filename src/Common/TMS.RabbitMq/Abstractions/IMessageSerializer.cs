namespace TMS.RabbitMq;

public interface IMessageSerializer 
{
    T? Deserialize<T>(ReadOnlySpan<byte> obj);

    byte[] SerializeToBytes(object item, Type inputType);
}
