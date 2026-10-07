namespace NorthWind.Sales.Entities.Dtos;

public record OrderInvoiceDto(
    int OrderId,
    DateTime OrderDate,
    string CustomerId,
    string CustomerName,
    string CustomerIdentification,
    string ShipAddress,
    string ShipCity,
    string ShipCountry,
    string ShipPostalCode,
    decimal Subtotal,
    decimal Tax,
    decimal Total,
    IReadOnlyList<OrderInvoiceDetailDto> Details);
