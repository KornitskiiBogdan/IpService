using Microsoft.EntityFrameworkCore;

namespace IpService.Dal.Ef;

public class DbContextBase : DbContext
{
    public const string DefaultConnectionStringName = "Default";

    public DbContextBase(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSnakeCaseNamingConvention();
}