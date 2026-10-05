using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NorthWind.DomainLogs.Entities.ValueObjects;
using NorthWind.Entities;
using NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
using NorthWind.Sales.Backend.DataContexts.EFCore.Services;
using NorthWind.Sales.Backend.Repositories.Interfaces;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

public class NorthWindDomainLogsContext(IOptions<DBOptions> dbOptions) : DbContext,
    INorthWindDomainLogsDataContext,
    IUnitOfWork
{
    private readonly DBOptions options = dbOptions.Value;

    public DbSet<DomainLog> DomainLogs => Set<DomainLog>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(options.NorthWindDomainLogsDB);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DomainLogConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public async Task AddLogAsync(DomainLog log, CancellationToken cancellationToken = default)
    {
        await DomainLogs.AddAsync(log, cancellationToken);
        await GuardDBContext.SaveChangesAsync(this, cancellationToken);
    }

    public new async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await GuardDBContext.SaveChangesAsync(this, cancellationToken);
    }
}
