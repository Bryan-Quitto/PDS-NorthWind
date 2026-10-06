namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

public class DomainLogConfiguration : IEntityTypeConfiguration<DomainLog>
{
    public void Configure(EntityTypeBuilder<DomainLog> builder)
    {
        builder.ToTable("DomainLogs");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.CreatedDate).IsRequired();
        builder.Property(l => l.Information).IsRequired();
        builder.Property(l => l.UserName).HasMaxLength(256);
    }
}
