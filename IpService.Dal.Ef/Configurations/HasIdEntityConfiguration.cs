using IpService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IpService.Dal.Ef.Configurations;

public abstract class HasIdEntityConfiguration<T, TId> : IEntityTypeConfiguration<T> where T : class, IHasIdEntity<TId>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(u => u.Id);
    }
}