using NorthWind.Sales.Backend.BusinessObjects.Aggregates;
using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

public interface ICommandsRepository
{
    Task CreateOrderAsync(OrderAggregate order, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrackedProduct>> GetProductsByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    void UpdateProducts(IEnumerable<TrackedProduct> products);
}
