using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Frontend.WebApiGateways;

public class OrderInvoiceGateway(HttpClient client) : IOrderInvoiceGateway
{
    public async Task<PaginatedResultDto<OrderSummaryDto>> SearchOrdersAsync(
        string? searchTerm,
        OrderSearchCriteria criteria = OrderSearchCriteria.All,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var parameters = new List<string>
        {
            $"criteria={Uri.EscapeDataString(criteria.ToString())}",
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            parameters.Add($"searchTerm={Uri.EscapeDataString(searchTerm.Trim())}");
        }

        var requestUri = $"{Endpoints.SearchOrders}?{string.Join("&", parameters)}";
        var response = await client.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginatedResultDto<OrderSummaryDto>>(cancellationToken);
        return result ?? new PaginatedResultDto<OrderSummaryDto>([], 0, pageNumber, pageSize);
    }

    public async Task<OrderInvoiceDto?> GetOrderInvoiceAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = Endpoints.GetOrderInvoice.Replace("{id}", orderId.ToString());
        var response = await client.GetAsync(requestUri, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OrderInvoiceDto>(cancellationToken);
    }

    public async Task<byte[]> GetOrderPdfAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = Endpoints.GetOrderPdf.Replace("{id}", orderId.ToString());
        var response = await client.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}
