using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NorthWind.Membership.Backend.AspNetIdentity.DataContexts;
using NorthWind.Membership.Backend.AspNetIdentity.Options;

namespace NorthWind.Membership.Backend.AspNetIdentity.Factories;

public class NorthWindMembershipContextFactory : IDesignTimeDbContextFactory<NorthWindMembershipContext>
{
    public NorthWindMembershipContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NorthWindMembershipContext>();
        const string connectionString = "Server=localhost;Database=NorthWindUsersDB;Trusted_Connection=True;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString);

        var dbOptions = Microsoft.Extensions.Options.Options.Create(new MembershipDBOptions { ConnectionString = connectionString });
        return new NorthWindMembershipContext(optionsBuilder.Options, dbOptions);
    }
}
