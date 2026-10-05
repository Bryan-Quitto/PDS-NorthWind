namespace NorthWind.Sales.Entities.Dtos;

public record CreateOrderDto(
    string CustomerId,
    string ShipAddress,
    string ShipCity,
    string ShipCountry,
    string ShipPostalCode,
    IReadOnlyList<CreateOrderDetailDto> OrderDetails);
