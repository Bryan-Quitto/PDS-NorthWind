namespace NorthWind.Sales.Frontend.Views.ViewModels;

public class CreateOrderDetailViewModel
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public short Quantity { get; set; } = 1;
    public string RawQuantity { get; set; } = "1";

    public decimal LineTotal => UnitPrice * Quantity;

    public CreateOrderDetailDto ToDto() => new(ProductId, UnitPrice, Quantity);
}
