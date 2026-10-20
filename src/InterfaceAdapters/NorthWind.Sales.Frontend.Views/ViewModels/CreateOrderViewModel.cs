namespace NorthWind.Sales.Frontend.Views.ViewModels;

public class CreateOrderViewModel
{
    public string CustomerId { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerIdentification { get; set; } = string.Empty;

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
    }

    public void AddOrUpdateDetail(int productId, string productName, decimal unitPrice, short quantity = 1, short unitsInStock = 0)
    {
        var existing = OrderDetails.FirstOrDefault(d => d.ProductId == productId);
        if (existing != null)
        {
            existing.Quantity = (short)(existing.Quantity + quantity);
            existing.RawQuantity = existing.Quantity.ToString();
            existing.UnitsInStock = unitsInStock;
        }
        else
        {
            OrderDetails.Add(new CreateOrderDetailViewModel
            {
                ProductId = productId,
                ProductName = productName,
                UnitPrice = unitPrice,
                Quantity = quantity,
                RawQuantity = quantity.ToString(),
                UnitsInStock = unitsInStock
            });
        }
    }

    public void ReplaceDetail(int oldProductId, int newProductId, string newProductName, decimal newUnitPrice, short newUnitsInStock = 0)
    {
        var existingIndex = OrderDetails.FindIndex(d => d.ProductId == oldProductId);
        if (existingIndex >= 0)
        {
            var alreadyInListIndex = OrderDetails.FindIndex(d => d.ProductId == newProductId);
            if (alreadyInListIndex >= 0 && alreadyInListIndex != existingIndex)
            {
                // Si el producto nuevo ya existía en otra fila, sumamos 1 a esa fila y quitamos la vieja
                OrderDetails[alreadyInListIndex].Quantity = (short)(OrderDetails[alreadyInListIndex].Quantity + 1);
                OrderDetails[alreadyInListIndex].RawQuantity = OrderDetails[alreadyInListIndex].Quantity.ToString();
                OrderDetails[alreadyInListIndex].UnitsInStock = newUnitsInStock;
                OrderDetails.RemoveAt(existingIndex);
            }
            else
            {
                OrderDetails[existingIndex] = new CreateOrderDetailViewModel
                {
                    ProductId = newProductId,
                    ProductName = newProductName,
                    UnitPrice = newUnitPrice,
                    Quantity = 1,
                    RawQuantity = "1",
                    UnitsInStock = newUnitsInStock
                };
            }
        }
    }

    public void UpdateQuantity(CreateOrderDetailViewModel detail, short newQuantity, string? rawQuantity = null)
    {
        detail.Quantity = newQuantity;
        detail.RawQuantity = rawQuantity ?? newQuantity.ToString();
    }

    public void ClearCustomer()
    {
        CustomerId = string.Empty;
        CustomerName = string.Empty;
        CustomerIdentification = string.Empty;
        ShipAddress = string.Empty;
        ShipCity = string.Empty;
        ShipCountry = string.Empty;
        ShipPostalCode = string.Empty;
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
