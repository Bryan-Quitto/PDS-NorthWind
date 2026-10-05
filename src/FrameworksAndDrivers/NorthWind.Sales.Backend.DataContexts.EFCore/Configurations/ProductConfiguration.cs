using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NorthWind.Sales.Backend.Repositories.Entities;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.UnitPrice).HasPrecision(18, 2);

        builder.HasData(
            new Product { Id = 1, Name = "Chai", UnitPrice = 35.00m, UnitsInStock = 20 },
            new Product { Id = 2, Name = "Chang", UnitPrice = 55.00m, UnitsInStock = 0 },
            new Product { Id = 3, Name = "Aniseed Syrup", UnitPrice = 65.00m, UnitsInStock = 20 },
            new Product { Id = 4, Name = "Chef Anton's Cajun Seasoning", UnitPrice = 75.00m, UnitsInStock = 40 },
            new Product { Id = 5, Name = "Chef Anton's Gumbo Mix", UnitPrice = 50.00m, UnitsInStock = 20 }
        );
    }
}
