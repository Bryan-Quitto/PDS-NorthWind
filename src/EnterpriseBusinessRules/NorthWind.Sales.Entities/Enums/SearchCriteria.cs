namespace NorthWind.Sales.Entities.Enums;

public enum CustomerSearchCriteria
{
    All = 0,
    Identification = 1,
    Name = 2,
    Id = 3,
    BusinessName = 4,
    LastName = 5
}

public enum ProductSearchCriteria
{
    All = 0,
    Name = 1,
    Id = 2
}

public enum OrderSearchCriteria
{
    All = 0,
    OrderId = 1,
    CustomerName = 2,
    CustomerIdentification = 3,
    CustomerId = 4
}
