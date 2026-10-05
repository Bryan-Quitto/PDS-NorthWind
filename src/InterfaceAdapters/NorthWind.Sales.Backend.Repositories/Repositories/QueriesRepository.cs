using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;
using NorthWind.Sales.Backend.Repositories.Interfaces;

namespace NorthWind.Sales.Backend.Repositories.Repositories;

public class QueriesRepository(INorthWindSalesQueriesDataContext context) : IQueriesRepository
{
    public Task<bool> CustomerExistsAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var exists = context.Customers.Any(c => c.Id == customerId);
        return Task.FromResult(exists);
    }

    public Task<IReadOnlyList<ProductUnitsInStock>> GetProductsUnitsInStockAsync(
        IEnumerable<int> productIds,
        CancellationToken cancellationToken = default)
    {
        var idList = productIds.ToList();
        var stock = context.Products
            .Where(p => idList.Contains(p.Id))
            .Select(p => new ProductUnitsInStock(p.Id, p.UnitsInStock))
            .ToList();

        return Task.FromResult<IReadOnlyList<ProductUnitsInStock>>(stock);
    }
}
