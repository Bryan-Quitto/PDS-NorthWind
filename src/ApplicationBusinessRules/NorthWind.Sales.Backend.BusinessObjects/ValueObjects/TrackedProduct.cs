namespace NorthWind.Sales.Backend.BusinessObjects.ValueObjects;

public class TrackedProduct
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
