namespace NorthWind.Sales.Backend.BusinessObjects.ValueObjects;

public readonly record struct ProductUnitsInStock(int ProductId, short UnitsInStock);
