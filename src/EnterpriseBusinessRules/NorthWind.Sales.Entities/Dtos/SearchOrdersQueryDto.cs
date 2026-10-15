using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Entities.Dtos;

public record SearchOrdersQueryDto(
    string? SearchTerm,
    OrderSearchCriteria Criteria = OrderSearchCriteria.All,
    int PageNumber = 1,
    int PageSize = 10);
