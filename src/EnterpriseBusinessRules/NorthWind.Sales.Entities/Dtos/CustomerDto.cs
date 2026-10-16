using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Entities.Dtos;

public record CustomerDto(
    string Id,
    string Name,
    string Identification,
    CustomerType CustomerType = CustomerType.Natural,
    string? FirstName = null,
    string? SecondName = null,
    string? LastName = null,
    string? SecondLastName = null,
    string? BusinessName = null,
    string? TradeName = null);
