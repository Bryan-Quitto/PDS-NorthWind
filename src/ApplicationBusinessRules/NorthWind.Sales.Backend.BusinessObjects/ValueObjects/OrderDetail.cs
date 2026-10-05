namespace NorthWind.Sales.Backend.BusinessObjects.ValueObjects;

public readonly record struct OrderDetail(int ProductId, decimal UnitPrice, short Quantity);
