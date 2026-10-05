namespace NorthWind.Sales.Frontend.Views.ViewModels;

public class CreateOrderViewModel
{
    public string CustomerId { get; set; } = string.Empty;

    public string ShipAddress { get; set; } = string.Empty;

    public string ShipCity { get; set; } = string.Empty;

    public string ShipCountry { get; set; } = string.Empty;

    public string ShipPostalCode { get; set; } = string.Empty;

    public List<CreateOrderDetailViewModel> OrderDetails { get; set; } = [];

    public void AddDetail(int productId = 0, decimal unitPrice = 0, short quantity = 1)
    {
        OrderDetails.Add(new CreateOrderDetailViewModel
        {
            ProductId = productId,
            UnitPrice = unitPrice,
            Quantity = quantity
        });
    }

    public void RemoveDetail(CreateOrderDetailViewModel detail)
    {
        OrderDetails.Remove(detail);
    }

    public void Reset()
    {
        CustomerId = string.Empty;
        ShipAddress = string.Empty;
        ShipCity = string.Empty;
        ShipCountry = string.Empty;
        ShipPostalCode = string.Empty;
        OrderDetails.Clear();
    }

    public CreateOrderDto ToDto() => new(
        CustomerId,
        ShipAddress,
        ShipCity,
        ShipCountry,
        ShipPostalCode,
        OrderDetails.Select(d => d.ToDto()).ToList()
    );
}
