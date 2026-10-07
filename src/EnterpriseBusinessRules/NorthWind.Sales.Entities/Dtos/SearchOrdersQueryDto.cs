namespace NorthWind.Sales.Entities.Dtos;

public record SearchOrdersQueryDto(
    string? SearchTerm,
    int PageNumber = 1,
    int PageSize = 10);
