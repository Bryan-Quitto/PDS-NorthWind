namespace NorthWind.Sales.Entities.Dtos;

public record OrderInvoiceDetailDto(
    int ProductId,
    string ProductName,
    decimal UnitPrice,
    short Quantity,
    decimal LineTotal);
