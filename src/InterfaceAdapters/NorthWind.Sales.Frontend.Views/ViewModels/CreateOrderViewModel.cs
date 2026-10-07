namespace NorthWind.Sales.Frontend.Views.ViewModels;

public class CreateOrderViewModel
{
    public string CustomerId { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerIdentification { get; set; } = string.Empty;

    public decimal? CustomerBalance { get; set; }

    public string ShipAddress { get; set; } = string.Empty;

    public string ShipCity { get; set; } = string.Empty;

    public string ShipCountry { get; set; } = string.Empty;

    public string ShipPostalCode { get; set; } = string.Empty;

    public List<CreateOrderDetailViewModel> OrderDetails { get; set; } = [];

    public decimal Subtotal => OrderDetails.Sum(d => d.UnitPrice * d.Quantity);

    public decimal Tax => decimal.Round(Subtotal * 0.15m, 2, MidpointRounding.AwayFromZero);

    public decimal Total => Subtotal + Tax;

    public void SetCustomer(CustomerDto customer)
    {
        CustomerId = customer.Id;
        CustomerName = customer.Name;
        CustomerIdentification = customer.Identification;
        CustomerBalance = customer.CurrentBalance;
    }

    public void AddOrUpdateDetail(int productId, string productName, decimal unitPrice, short quantity = 1)
    {
        var existing = OrderDetails.FirstOrDefault(d => d.ProductId == productId);
        if (existing != null)
        {
            existing.Quantity = (short)(existing.Quantity + quantity);
        }
        else
        {
            OrderDetails.Add(new CreateOrderDetailViewModel
            {
                ProductId = productId,
                ProductName = productName,
                UnitPrice = unitPrice,
                Quantity = quantity
            });
        }
    }

    public void UpdateQuantity(CreateOrderDetailViewModel detail, short newQuantity)
    {
        if (newQuantity <= 0)
        {
            RemoveDetail(detail);
        }
        else
        {
            detail.Quantity = newQuantity;
        }
    }

    public void RemoveDetail(CreateOrderDetailViewModel detail)
    {
        OrderDetails.Remove(detail);
    }

    public void Reset()
    {
        CustomerId = string.Empty;
        CustomerName = string.Empty;
        CustomerIdentification = string.Empty;
        CustomerBalance = null;
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
