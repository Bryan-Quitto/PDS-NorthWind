using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Backend.Repositories.Entities;

public class Customer
{
    public string Id { get; set; } = string.Empty;
    public CustomerType CustomerType { get; set; } = CustomerType.Natural;
    public string Identification { get; set; } = string.Empty;

    // Campos 1FN
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? LastName { get; set; }
    public string? SecondLastName { get; set; }
    public string? BusinessName { get; set; }
    public string? TradeName { get; set; }

    // Columna calculada persistida en BD
    public string Name { get; private set; } = string.Empty;
}
