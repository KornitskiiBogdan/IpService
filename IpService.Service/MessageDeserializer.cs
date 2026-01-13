using Confluent.Kafka;
using System.Text.Json;

namespace IpService.Service
{
    public sealed class MessageDeserializer<TMessage> : IDeserializer<TMessage>
    {

        public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return default;
            }

            return JsonSerializer.Deserialize<TMessage>(data, JsonSerializerOptions.Web);
           
        }
    }
}
