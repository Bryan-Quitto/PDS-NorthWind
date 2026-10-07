namespace NorthWind.Sales.Entities.Dtos;

public record SearchProductsQueryDto(string? SearchTerm, int PageNumber = 1, int PageSize = 10);
