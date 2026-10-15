using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Entities.Dtos;

public record SearchCustomersQueryDto(
    string? SearchTerm,
    CustomerSearchCriteria Criteria = CustomerSearchCriteria.All,
    int PageNumber = 1,
    int PageSize = 10);
