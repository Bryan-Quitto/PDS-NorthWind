using NorthWind.Sales.Backend.BusinessObjects.Aggregates;
using NorthWind.Sales.Backend.BusinessObjects.Entities;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.Repositories.Interfaces;

namespace NorthWind.Sales.Backend.Repositories.Repositories;

public class CommandsRepository(INorthWindSalesCommandsDataContext context) : ICommandsRepository
{
    public async Task CreateOrderAsync(OrderAggregate order, CancellationToken cancellationToken = default)
    {
        var orderEntity = new Order
        {
            CustomerId = order.CustomerId,
            ShipAddress = order.ShipAddress,
            ShipCity = order.ShipCity,
            ShipCountry = order.ShipCountry,
            ShipPostalCode = order.ShipPostalCode,
            ShippingType = order.ShippingType,
            DiscountType = order.DiscountType,
            Discount = order.Discount,
            OrderDate = order.OrderDate
        };

        await context.AddOrderAsync(orderEntity, cancellationToken);

        // Al persistirse el contexto se sincroniza el Id generado
        order.Id = orderEntity.Id;

        var details = order.OrderDetails.Select(d => new Entities.OrderDetail
        {
            OrderId = orderEntity.Id,
            ProductId = d.ProductId,
            UnitPrice = d.UnitPrice,
            Quantity = d.Quantity
        }).ToList();

        await context.AddOrderDetailsAsync(details, cancellationToken);
    }
}
