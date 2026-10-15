namespace NorthWind.Sales.Entities.Dtos;

public record OrderSummaryDto(
    int OrderId,
    DateTime OrderDate,
    string CustomerId,
    string CustomerName,
    string CustomerIdentification,
    int ItemsCount,
    decimal Total);
