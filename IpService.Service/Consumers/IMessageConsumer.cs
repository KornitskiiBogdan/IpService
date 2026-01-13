namespace IpService.Service.Consumers;

public interface IMessageConsumer<in TMessage> where TMessage : class
{
    public Task ConsumeAsync(TMessage message, CancellationToken cancellationToken = default);
}