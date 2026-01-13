using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IpService.Dal.Ef;

public static class HostExtensions
{
    public static async Task RunWithMigrationAsync(this IHost host, CancellationToken token = default)
    {
        await using (var scope = host.Services.CreateAsyncScope())
        {
            var migrationService = scope.ServiceProvider.GetService<IMigrationService>();

            if (migrationService != null)
            {
                await migrationService.MigrateAsync(token);
            }
        }

        await host.RunAsync(token);
    }
}