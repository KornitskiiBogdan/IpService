namespace IpService.Service.Consumers;

public class KafkaConfig<T>
{
    public required string Topic { get; init; }
    public int ConsumerNumber { get; init; }
}