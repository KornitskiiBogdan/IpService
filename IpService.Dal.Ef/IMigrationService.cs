namespace IpService.Dal.Ef;

public interface IMigrationService
{
    Task MigrateAsync(CancellationToken cancellationToken);
}