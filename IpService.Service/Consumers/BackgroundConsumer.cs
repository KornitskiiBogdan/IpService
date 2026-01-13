using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace IpService.Service.Consumers
{
    public class BackgroundConsumer<TKey, TMessage> : BackgroundService
    {
        private readonly string _topic;
        private readonly int _consumerNumber;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<IServiceScope> _scopes = new();

        public BackgroundConsumer(IServiceProvider serviceProvider, IOptions<KafkaConfig<TMessage>> config)
        {
            var topic = config.Value.Topic;
            var consumerNumber = config.Value.ConsumerNumber;

            _topic = topic ?? throw new ArgumentNullException(nameof(topic));

            if (consumerNumber < 1) throw new ArgumentOutOfRangeException(nameof(consumerNumber));

            _consumerNumber = consumerNumber;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _scopes.AddRange(Enumerable.Range(0, _consumerNumber).Select(_ => _serviceProvider.CreateScope()));

            return Task.WhenAll(_scopes.Select(s =>
            {
                var consumer = s.ServiceProvider.GetRequiredService<IKafkaConsumer<TKey, TMessage>>();

                return Task.Run(() => consumer.SubscribeAsync(_topic, stoppingToken), stoppingToken);
            }));
        }

        public override void Dispose()
        {
            base.Dispose();

            _scopes.ForEach(s => s.Dispose());
        }
    }
}
