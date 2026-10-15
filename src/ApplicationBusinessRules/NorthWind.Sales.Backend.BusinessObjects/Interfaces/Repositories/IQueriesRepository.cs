using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;
using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

public interface IQueriesRepository
{
    Task<bool> CustomerExistsAsync(string customerId, CancellationToken cancellationToken = default);
    Task<decimal> GetCustomerCurrentBalanceAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductUnitsInStock>> GetProductsUnitsInStockAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default);
    Task<PaginatedResultDto<CustomerDto>> SearchCustomersAsync(string? searchTerm, CustomerSearchCriteria criteria, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PaginatedResultDto<ProductDto>> SearchProductsAsync(string? searchTerm, ProductSearchCriteria criteria, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<OrderInvoiceDto?> GetOrderInvoiceByIdAsync(int orderId, CancellationToken cancellationToken = default);
    Task<PaginatedResultDto<OrderSummaryDto>> SearchOrdersAsync(string? searchTerm, OrderSearchCriteria criteria, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
