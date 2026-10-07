namespace NorthWind.Sales.Entities.Dtos;

public record PaginatedResultDto<T>(IReadOnlyList<T> Items, int TotalCount, int PageNumber, int PageSize);
