using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NorthWind.DomainLogs.Entities.ValueObjects;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

public class DomainLogConfiguration : IEntityTypeConfiguration<DomainLog>
{
    public void Configure(EntityTypeBuilder<DomainLog> builder)
    {
        builder.ToTable("DomainLogs");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Action).IsRequired().HasMaxLength(100);
        builder.Property(l => l.Description).IsRequired().HasMaxLength(500);
        builder.Property(l => l.User).IsRequired().HasMaxLength(50);
    }
}
