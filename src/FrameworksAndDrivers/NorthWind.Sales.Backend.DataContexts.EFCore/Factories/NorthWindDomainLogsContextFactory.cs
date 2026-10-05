using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Factories;

public class NorthWindDomainLogsContextFactory : IDesignTimeDbContextFactory<NorthWindDomainLogsContext>
{
    public NorthWindDomainLogsContext CreateDbContext(string[] args)
    {
        var options = Microsoft.Extensions.Options.Options.Create(new DBOptions
        {
            DomainLogsConnectionString = "Server=localhost;Database=NorthWindLogsDB;Trusted_Connection=True;TrustServerCertificate=True;"
        });
        return new NorthWindDomainLogsContext(options);
    }
}
