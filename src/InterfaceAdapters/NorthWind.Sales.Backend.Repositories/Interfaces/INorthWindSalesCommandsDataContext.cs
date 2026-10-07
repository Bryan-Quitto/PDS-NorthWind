namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesCommandsDataContext
{
    Task<IReadOnlyList<Entities.Product>> GetProductsByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task AddOrderDetailsAsync(IEnumerable<Entities.OrderDetail> orderDetails, CancellationToken cancellationToken = default);
    void UpdateProducts(IEnumerable<Entities.Product> products);
}
