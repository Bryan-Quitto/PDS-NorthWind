using NorthWind.Sales.Backend.Repositories.Entities;

namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesQueriesDataContext
{
    IQueryable<Customer> Customers { get; }
    IQueryable<Product> Products { get; }
}
