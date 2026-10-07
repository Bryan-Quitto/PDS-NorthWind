namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ISearchGateway
{
    Task<PaginatedResultDto<CustomerDto>> SearchCustomersAsync(
        string? searchTerm,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<PaginatedResultDto<ProductDto>> SearchProductsAsync(
        string? searchTerm,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}
