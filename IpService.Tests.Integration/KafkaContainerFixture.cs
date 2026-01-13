
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Testcontainers.Kafka;
using ContainerBuilder = DotNet.Testcontainers.Builders.ContainerBuilder;

namespace IpService.Tests.Integration
{
    public class KafkaContainerFixture
    {
        private KafkaContainer? _kafka;
        private readonly KafkaBuilder _kafkaBuilder;
        private ContainerBuilder _schemaRegistryBuilder;
        private INetwork _testContainerNetwork;
        private IContainer? _schemaRegistryContainer;


        public string? BootstrapServers { get; private set; }
        public string? SchemaRegistryUrl { get; private set; }

        public KafkaContainerFixture()
        {
            var brokerInternalPort = 19092;
            var schemaRegistryInternalPort = 18083;

            _testContainerNetwork = new NetworkBuilder().Build();
            
            _kafkaBuilder = new KafkaBuilder("confluentinc/cp-kafka:6.2.10")
                .WithCleanUp(true)
                .WithNetworkAliases("kafka")
                .WithListener($"kafka:{brokerInternalPort}");

            _schemaRegistryBuilder = new ContainerBuilder()
                .WithImage("confluentinc/cp-schema-registry:7.5.2")
                .WithPortBinding(schemaRegistryInternalPort, true)
                .WithEnvironment("SCHEMA_REGISTRY_HOST_NAME", "schema-registry")
                .WithEnvironment("SCHEMA_REGISTRY_LISTENERS", $"http://0.0.0.0:{schemaRegistryInternalPort}")
                .WithEnvironment("SCHEMA_REGISTRY_KAFKASTORE_BOOTSTRAP_SERVERS", $"PLAINTEXT://kafka:{brokerInternalPort}")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(schemaRegistryInternalPort));
        }

        public async Task StartContainerAsync()
        {
            await _testContainerNetwork.CreateAsync();

            _kafka = _kafkaBuilder
                .WithNetwork(_testContainerNetwork)
                .Build();

            await _kafka.StartAsync();
            BootstrapServers = _kafka.GetBootstrapAddress();

            _schemaRegistryContainer = _schemaRegistryBuilder
                .WithNetwork(_testContainerNetwork)
                .Build();

            await _schemaRegistryContainer.StartAsync();

            var schemaRegistryPort = _schemaRegistryContainer.GetMappedPublicPort();
            SchemaRegistryUrl = $"http://localhost:{schemaRegistryPort}";
        }

        public async Task StopContainerAsync()
        {
            await _testContainerNetwork.DeleteAsync();
            await _kafka.DisposeAsync();
            await _schemaRegistryContainer.DisposeAsync();
        }
    }
}
