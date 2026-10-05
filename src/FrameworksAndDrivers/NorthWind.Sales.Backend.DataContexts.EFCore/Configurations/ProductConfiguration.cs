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
            new Product { Id = 1, Name = "Chai", UnitPrice = 18.00m, UnitsInStock = 39 },
            new Product { Id = 2, Name = "Chang", UnitPrice = 19.00m, UnitsInStock = 17 },
            new Product { Id = 3, Name = "Aniseed Syrup", UnitPrice = 10.00m, UnitsInStock = 13 },
            new Product { Id = 4, Name = "Chef Anton's Cajun Seasoning", UnitPrice = 22.00m, UnitsInStock = 53 },
            new Product { Id = 5, Name = "Chef Anton's Gumbo Mix", UnitPrice = 21.35m, UnitsInStock = 0 }
        );
    }
}
