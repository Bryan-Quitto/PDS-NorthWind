using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

public interface IQueriesRepository
{
    Task<bool> CustomerExistsAsync(string customerId, CancellationToken cancellationToken = default);
    Task<decimal> GetCustomerCurrentBalanceAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductUnitsInStock>> GetProductsUnitsInStockAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default);
}
