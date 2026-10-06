namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("OrderDetails");
        builder.HasKey(od => new { od.OrderId, od.ProductId });
        builder.Property(od => od.UnitPrice).HasPrecision(18, 2);

        builder.HasOne(od => od.Order)
            .WithMany()
            .HasForeignKey(od => od.OrderId);
    }
}
