using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Factories;

public class NorthWindSalesContextFactory : IDesignTimeDbContextFactory<NorthWindSalesContext>
{
    public NorthWindSalesContext CreateDbContext(string[] args)
    {
        var options = Microsoft.Extensions.Options.Options.Create(new DBOptions
        {
            ConnectionString = "Server=localhost;Database=NorthWindDB;Trusted_Connection=True;TrustServerCertificate=True;"
        });
        return new NorthWindSalesContext(options);
    }
}
