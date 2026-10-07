using NorthWind.Sales.Backend.BusinessObjects.Entities;
using NorthWind.Sales.Backend.BusinessObjects.Enums;
using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;

namespace NorthWind.Sales.Backend.BusinessObjects.Aggregates;

public class OrderAggregate : Order
{
    private readonly List<OrderDetail> orderDetailsField = [];

    public IReadOnlyCollection<OrderDetail> OrderDetails => orderDetailsField.AsReadOnly();

    public const decimal TaxRate = 0.15m;

    public void AddDetail(int productId, decimal unitPrice, short quantity)
    {
        var existingIndex = orderDetailsField.FindIndex(d => d.ProductId == productId);
        if (existingIndex >= 0)
        {
            var existing = orderDetailsField[existingIndex];
            orderDetailsField[existingIndex] = new OrderDetail(
                productId,
                unitPrice,
                (short)(existing.Quantity + quantity)
            );
        }
        else
        {
            orderDetailsField.Add(new OrderDetail(productId, unitPrice, quantity));
        }

        RecalculateTotals();
    }

    private void RecalculateTotals()
    {
        Subtotal = orderDetailsField.Sum(d => d.UnitPrice * d.Quantity);
        Tax = Math.Round(Subtotal * TaxRate, 2, MidpointRounding.AwayFromZero);
        Total = Subtotal + Tax;
    }

    public static OrderAggregate From(CreateOrderDto orderDto)
    {
        var aggregate = new OrderAggregate
        {
            CustomerId = orderDto.CustomerId,
            ShipAddress = orderDto.ShipAddress,
            ShipCity = orderDto.ShipCity,
            ShipCountry = orderDto.ShipCountry,
            ShipPostalCode = orderDto.ShipPostalCode,
            ShippingType = ShippingType.Road,
            DiscountType = DiscountType.Percentage,
            Discount = 10,
            OrderDate = DateTime.UtcNow
        };

        if (orderDto.OrderDetails != null)
        {
            foreach (var detail in orderDto.OrderDetails)
            {
                aggregate.AddDetail(detail.ProductId, detail.UnitPrice, detail.Quantity);
            }
        }

        return aggregate;
    }
}
