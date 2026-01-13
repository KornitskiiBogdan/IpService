using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IpService.Dal.Ef;

public sealed class MigrationService<T> : IMigrationService where T : DbContextBase
{
    private readonly ILogger<MigrationService<T>> _logger;
    private readonly T _dbContext;
    private readonly IConfiguration _configuration;

    public MigrationService(ILogger<MigrationService<T>> logger,
        T dbContext,
        IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Migration started");

        const string migrationConnectionStringName = "Migration";

        var connectionString = _configuration.GetConnectionString(migrationConnectionStringName);

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            await _dbContext.Database.CloseConnectionAsync();
            _dbContext.Database.SetConnectionString(connectionString);
        }

        await _dbContext.Database.MigrateAsync(cancellationToken);

        _logger.LogDebug("Migration ended");
    }
}