namespace NorthWind.Sales.Entities.Dtos;

public record CreateOrderDetailDto(int ProductId, decimal UnitPrice, short Quantity);
