namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(5).IsFixedLength();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Identification).IsRequired().HasMaxLength(10).IsFixedLength();

        builder.HasData(
            new Customer { Id = "ALFKI", Name = "Alfreds Futterkiste", Identification = "0102030405" },
            new Customer { Id = "ANATR", Name = "Ana Trujillo Emparedados y helados", Identification = "0203040506" },
            new Customer { Id = "ANTON", Name = "Antonio Moreno Taqueria", Identification = "0304050607" },
            new Customer { Id = "AROUT", Name = "Around the Horn", Identification = "0405060708" },
            new Customer { Id = "BERGS", Name = "Berglunds snabbkop", Identification = "0506070809" }
        );
    }
}
