namespace IpService.Service.Consumers;

public interface IKafkaConsumer<TKey, TMessage>
{
    Task SubscribeAsync(string topic, CancellationToken cancellationToken);
}