namespace NorthWind.Sales.Entities.Dtos;

public record ProductDto(int Id, string Name, decimal UnitPrice, short UnitsInStock);
