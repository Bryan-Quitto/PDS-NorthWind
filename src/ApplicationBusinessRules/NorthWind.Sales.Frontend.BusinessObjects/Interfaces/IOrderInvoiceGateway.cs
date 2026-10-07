namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface IOrderInvoiceGateway
{
    Task<PaginatedResultDto<OrderSummaryDto>> SearchOrdersAsync(
        string? searchTerm,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<OrderInvoiceDto?> GetOrderInvoiceAsync(
        int orderId,
        CancellationToken cancellationToken = default);

    Task<byte[]> GetOrderPdfAsync(
        int orderId,
        CancellationToken cancellationToken = default);
}
