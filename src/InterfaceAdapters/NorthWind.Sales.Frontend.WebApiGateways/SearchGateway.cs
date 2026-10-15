using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Frontend.WebApiGateways;

public class SearchGateway(HttpClient client) : ISearchGateway
{
    public async Task<PaginatedResultDto<CustomerDto>> SearchCustomersAsync(
        string? searchTerm,
        CustomerSearchCriteria criteria = CustomerSearchCriteria.All,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString(searchTerm, criteria.ToString(), pageNumber, pageSize);
        var requestUri = $"{Endpoints.SearchCustomers}{queryString}";

        var response = await client.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginatedResultDto<CustomerDto>>(cancellationToken);
        return result ?? new PaginatedResultDto<CustomerDto>([], 0, pageNumber, pageSize);
    }

    public async Task<PaginatedResultDto<ProductDto>> SearchProductsAsync(
        string? searchTerm,
        ProductSearchCriteria criteria = ProductSearchCriteria.All,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString(searchTerm, criteria.ToString(), pageNumber, pageSize);
        var requestUri = $"{Endpoints.SearchProducts}{queryString}";

        var response = await client.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginatedResultDto<ProductDto>>(cancellationToken);
        return result ?? new PaginatedResultDto<ProductDto>([], 0, pageNumber, pageSize);
    }

    private static string BuildQueryString(string? searchTerm, string criteria, int pageNumber, int pageSize)
    {
        var parameters = new List<string>
        {
            $"criteria={Uri.EscapeDataString(criteria)}",
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            parameters.Add($"searchTerm={Uri.EscapeDataString(searchTerm.Trim())}");
        }

        return $"?{string.Join("&", parameters)}";
    }
}
