namespace NorthWind.Sales.Entities.Dtos;

public record SearchCustomersQueryDto(string? SearchTerm, int PageNumber = 1, int PageSize = 10);
