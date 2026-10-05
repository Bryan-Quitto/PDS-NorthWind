using NorthWind.Sales.Backend.BusinessObjects.Aggregates;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

public interface ICommandsRepository
{
    Task CreateOrderAsync(OrderAggregate order, CancellationToken cancellationToken = default);
}
