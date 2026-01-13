using IpService.Dal.Ef;
using Microsoft.EntityFrameworkCore;

namespace IpService.Dal
{
    public sealed class UserIpContext(DbContextOptions options) : DbContextBase(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserIpContext).Assembly);
        }
    }
}
