using IpService.Dal;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IpService.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private PostgreSqlContainerFixture _pgContainer;
        private KafkaContainerFixture _kafkaContainer;
        private readonly string _environment; 
        public CustomWebApplicationFactory(string environment = "Development")
        {
            _environment = environment;
            _pgContainer = new PostgreSqlContainerFixture();
            _kafkaContainer = new KafkaContainerFixture();
        }

        public string BootstrapServers { get; private set; }
        public string? SchemaRegistryUrl { get; private set; }

        public async Task InitializeAsync()
        {
            await _pgContainer.StartContainerAsync();
            await _kafkaContainer.StartContainerAsync();

            BootstrapServers = _kafkaContainer.BootstrapServers ?? throw new ArgumentNullException();
            SchemaRegistryUrl = _kafkaContainer.SchemaRegistryUrl ?? throw new ArgumentNullException();
        }

        public override async ValueTask DisposeAsync()
        {
            await _pgContainer.StopContainerAsync();
            await _kafkaContainer.StopContainerAsync();
            
            await base.DisposeAsync();
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(_environment);

            builder.ConfigureServices(services =>
            {
                var descriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UserIpContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<UserIpContext>(options => { options.UseNpgsql(_pgContainer.ConnectionString); });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<UserIpContext>();
                db.Database.EnsureCreated();
            });

            return base.CreateHost(builder);
        }
    }
}
