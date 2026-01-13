using IpService.Dal.Ef.Configurations;
using IpService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IpService.Dal.Configuration
{
    public class UserIpConfiguration : HasIdEntityConfiguration<UserIp, Guid>
    {
        public override void Configure(EntityTypeBuilder<UserIp> builder)
        {
            base.Configure(builder);

            builder.ToTable("user_ips");

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.UserId).IsRequired();

            builder.Property(p => p.IpAddress).IsRequired();

            builder.Property(p => p.LastConnectedAt).IsRequired();


        }
    }
}
