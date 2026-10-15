using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Entities.Dtos;

public record SearchProductsQueryDto(
    string? SearchTerm,
    ProductSearchCriteria Criteria = ProductSearchCriteria.All,
    int PageNumber = 1,
    int PageSize = 10);
