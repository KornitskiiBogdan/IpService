using System.Text.Json;
using Confluent.Kafka;

namespace IpService.Service;

public sealed class MessageSerializer<TMessage> : ISerializer<TMessage>
{
    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data, JsonSerializerOptions.Web);
    }
}