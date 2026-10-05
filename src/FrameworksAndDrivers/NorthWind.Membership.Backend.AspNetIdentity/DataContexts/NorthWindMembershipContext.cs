using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NorthWind.Membership.Backend.AspNetIdentity.Entities;
using NorthWind.Membership.Backend.AspNetIdentity.Options;

namespace NorthWind.Membership.Backend.AspNetIdentity.DataContexts;

public class NorthWindMembershipContext : IdentityDbContext<NorthWindUser>
{
    private readonly MembershipDBOptions? options;

    public NorthWindMembershipContext(IOptions<MembershipDBOptions> dbOptions)
    {
        options = dbOptions.Value;
    }

    public NorthWindMembershipContext(
        DbContextOptions<NorthWindMembershipContext> dbContextOptions,
        IOptions<MembershipDBOptions> dbOptions)
        : base(dbContextOptions)
    {
        options = dbOptions.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && options != null && !string.IsNullOrEmpty(options.ConnectionString))
        {
            optionsBuilder.UseSqlServer(options.ConnectionString);
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<NorthWindUser>(b =>
        {
            b.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            b.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        });
    }
}
