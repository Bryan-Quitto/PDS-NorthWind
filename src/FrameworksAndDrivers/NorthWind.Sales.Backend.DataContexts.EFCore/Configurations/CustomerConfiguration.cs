namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(5).IsFixedLength();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.Property(c => c.CurrentBalance).HasPrecision(18, 2);

        builder.HasData(
            new Customer { Id = "ALFKI", Name = "Alfreds Futterkiste", CurrentBalance = 0m },
            new Customer { Id = "ANATR", Name = "Ana Trujillo Emparedados y helados", CurrentBalance = 0m },
            new Customer { Id = "ANTON", Name = "Antonio Moreno Taqueria", CurrentBalance = 100.00m },
            new Customer { Id = "AROUT", Name = "Around the Horn", CurrentBalance = 0m },
            new Customer { Id = "BERGS", Name = "Berglunds snabbkop", CurrentBalance = 0m }
        );
    }
}
