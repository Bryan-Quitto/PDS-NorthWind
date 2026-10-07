namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.CustomerId).IsRequired().HasMaxLength(5).IsFixedLength();
        builder.Property(o => o.ShipAddress).IsRequired().HasMaxLength(60);
        builder.Property(o => o.ShipCity).IsRequired().HasMaxLength(15);
        builder.Property(o => o.ShipCountry).IsRequired().HasMaxLength(15);
        builder.Property(o => o.ShipPostalCode).IsRequired().HasMaxLength(10);
        builder.Property(o => o.ShippingType).IsRequired();
        builder.Property(o => o.DiscountType).IsRequired();
        builder.Property(o => o.Discount).HasPrecision(8, 2);
        builder.Property(o => o.Subtotal).HasPrecision(18, 2);
        builder.Property(o => o.Tax).HasPrecision(18, 2);
        builder.Property(o => o.Total).HasPrecision(18, 2);
    }
}
