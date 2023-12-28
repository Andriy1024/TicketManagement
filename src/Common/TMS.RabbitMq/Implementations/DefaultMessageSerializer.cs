using System.Text.Json;

namespace TMS.RabbitMq.Implementations;

public class DefaultMessageSerializer : IMessageSerializer
{
    private readonly JsonSerializerOptions _serializerOptions;

    public DefaultMessageSerializer()
    {
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public T? Deserialize<T>(ReadOnlySpan<byte> obj)
    {
        return JsonSerializer.Deserialize<T>(obj, _serializerOptions);
    }

    public byte[] SerializeToBytes(object item, Type inputType)
    {
        return JsonSerializer.SerializeToUtf8Bytes(item, inputType, _serializerOptions);
    }
}
