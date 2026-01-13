using Confluent.Kafka;
using IpService.Contracts;
using IpService.Dal.Ef.QueryProvider;
using IpService.Dal.Queries.Filters;
using IpService.Dal.Queries.Materializers;
using IpService.Domain.Entities;
using IpService.Service;
using IpService.Service.Consumers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;

namespace IpService.Tests.Integration
{
    [TestFixture]
    public class KafkaConsumerTests
    {
        private CustomWebApplicationFactory _factory;
        private IProducer<string, UserIpEventMessage> _producer;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _factory = new CustomWebApplicationFactory();
            await _factory.InitializeAsync();
            var bootstrapServers = _factory.BootstrapServers;

            _producer = new ProducerBuilder<string, UserIpEventMessage>(new ProducerConfig
                {
                    BootstrapServers = bootstrapServers
                })
                .SetValueSerializer(new MessageSerializer<UserIpEventMessage>())
                .Build();
        }

        [Test]
        public async Task GivenMessageIsProduced_ConsumerProcessesItSuccessfully()
        {
            var topicName = "ip-topic";
            var message = new UserIpEventMessage()
            {
                IpAddress = "127.0.0.1",
                LastConnectedAt = DateTimeOffset.Now,
                UserId = 37
            };

            await using var scope = _factory.Services.CreateAsyncScope();
            var queryProvider = scope.ServiceProvider.GetRequiredService<IQueryProvider<UserIp>>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var mockLogger = new Mock<ILogger<KafkaConsumer<string, UserIpEventMessage>>>();
            var mockConsuemrLogger = new Mock<ILogger<MessageConsumer>>();

            var options = Options.Create(new KafkaSection() { Uri = _factory.BootstrapServers });
            var consumer = new KafkaConsumer<string, UserIpEventMessage>(options, mockLogger.Object,
                new MessageDeserializer<UserIpEventMessage>(),
                new MessageConsumer(mediator, mockConsuemrLogger.Object));

            await _producer.ProduceAsync(topicName,
                new Message<string, UserIpEventMessage>() { Value = message, Key = string.Empty });

            var cancellationTokenSource = new CancellationTokenSource();
            var consumerSubscription = consumer.SubscribeAsync(topicName, cancellationTokenSource.Token);

            cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(500));
            await consumerSubscription;

            var users = await queryProvider.ProjectAsync(UserIpMaterializers.SelectByFilter<UserIp>(UserIpFilters.ByUserId(message.UserId)), token: CancellationToken.None);
            var user = users.SingleOrDefault();

            ArgumentNullException.ThrowIfNull(user);
            user.IpAddress.ShouldBe("127.0.0.1");
            user.UserId.ShouldBe(37);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            _producer.Dispose();
            await _factory.DisposeAsync();
        }
    }
}
