using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NorthWind.Sales.Backend.Repositories.Entities;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(5).IsFixedLength();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);

        builder.HasData(
            new Customer { Id = "ALFKI", Name = "Alfreds Futterkiste" },
            new Customer { Id = "ANATR", Name = "Ana Trujillo Emparedados y helados" },
            new Customer { Id = "ANTON", Name = "Antonio Moreno Taquería" },
            new Customer { Id = "AROUT", Name = "Around the Horn" },
            new Customer { Id = "BERGS", Name = "Berglunds snabbköp" }
        );
    }
}
