using IpService.Dal;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace IpService.Tests.Integration
{
    public class PostgreSqlContainerFixture
    {
        private PostgreSqlContainer _postgreSqlContainer;
        public string ConnectionString { get; private set; }

        public PostgreSqlContainerFixture()
        {
            var dbName = $"testdb_{Guid.NewGuid():N}";

            _postgreSqlContainer = new PostgreSqlBuilder("postgres:16")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithDatabase(dbName)
                .WithPortBinding(5432, true)
                .Build();
        }

        public async Task StartContainerAsync()
        {
            await _postgreSqlContainer.StartAsync();

            ConnectionString = _postgreSqlContainer.GetConnectionString();

            await ApplyMigrationsAsync();
        }

        public async Task StopContainerAsync()
        {
            await _postgreSqlContainer.DisposeAsync();
        }

        private async Task ApplyMigrationsAsync()
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserIpContext>();
            optionsBuilder.UseNpgsql(ConnectionString);

            await using var context = new UserIpContext(optionsBuilder.Options);

            await context.Database.MigrateAsync();
        }
    }
}
