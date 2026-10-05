using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NorthWind.DomainLogs.Entities.ValueObjects;
using NorthWind.Entities;
using NorthWind.Sales.Backend.BusinessObjects.Entities;
using NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
using NorthWind.Sales.Backend.DataContexts.EFCore.Services;
using NorthWind.Sales.Backend.Repositories.Entities;
using NorthWind.Sales.Backend.Repositories.Interfaces;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

public class NorthWindSalesContext(IOptions<DBOptions> dbOptions) : DbContext,
    INorthWindSalesCommandsDataContext,
    INorthWindSalesQueriesDataContext,
    IUnitOfWork
{
    private readonly DBOptions options = dbOptions.Value;

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();

    IQueryable<Customer> INorthWindSalesQueriesDataContext.Customers => Customers.AsNoTracking();
    IQueryable<Product> INorthWindSalesQueriesDataContext.Products => Products.AsNoTracking();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=NorthWindDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
        modelBuilder.Entity<NorthWind.Sales.Backend.BusinessObjects.Aggregates.OrderAggregate>().HasBaseType<Order>();
        base.OnModelCreating(modelBuilder);
    }

    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        await Orders.AddAsync(order, cancellationToken);
    }

    public async Task AddOrderDetailsAsync(IEnumerable<OrderDetail> orderDetails, CancellationToken cancellationToken = default)
    {
        await OrderDetails.AddRangeAsync(orderDetails, cancellationToken);
    }

    public new async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await GuardDBContext.SaveChangesAsync(this, cancellationToken);
    }
}
