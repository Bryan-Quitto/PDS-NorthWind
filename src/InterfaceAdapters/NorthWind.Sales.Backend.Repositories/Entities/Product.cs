namespace NorthWind.Sales.Backend.Repositories.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
}
