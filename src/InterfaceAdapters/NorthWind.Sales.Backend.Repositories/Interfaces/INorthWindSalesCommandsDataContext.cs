using NorthWind.Sales.Backend.BusinessObjects.Entities;
using NorthWind.Sales.Backend.Repositories.Entities;

namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesCommandsDataContext
{
    Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task AddOrderDetailsAsync(IEnumerable<Entities.OrderDetail> orderDetails, CancellationToken cancellationToken = default);
}
