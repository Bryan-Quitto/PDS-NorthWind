namespace NorthWind.Sales.Entities.Dtos;

public record OrderSummaryDto(
    int OrderId,
    DateTime OrderDate,
    string CustomerId,
    string CustomerName,
    int ItemsCount,
    decimal Total);
