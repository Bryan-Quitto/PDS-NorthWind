using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ISearchGateway
{
    Task<PaginatedResultDto<CustomerDto>> SearchCustomersAsync(
        string? searchTerm,
        CustomerSearchCriteria criteria = CustomerSearchCriteria.All,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<PaginatedResultDto<ProductDto>> SearchProductsAsync(
        string? searchTerm,
        ProductSearchCriteria criteria = ProductSearchCriteria.All,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}
