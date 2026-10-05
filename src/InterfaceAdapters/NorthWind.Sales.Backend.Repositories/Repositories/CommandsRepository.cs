using NorthWind.Sales.Backend.BusinessObjects.Aggregates;
using NorthWind.Sales.Backend.BusinessObjects.Entities;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.Repositories.Interfaces;

namespace NorthWind.Sales.Backend.Repositories.Repositories;

public class CommandsRepository(INorthWindSalesCommandsDataContext context) : ICommandsRepository
{
    public async Task CreateOrderAsync(OrderAggregate order, CancellationToken cancellationToken = default)
    {
        // 1. Agregar la instancia de la orden al contexto de EF Core
        await context.AddOrderAsync(order, cancellationToken);

        // 2. Vincular cada detalle directamente a la referencia en memoria 'order' (no a un escalar OrderId todavía no generado)
        var details = order.OrderDetails.Select(d => new Entities.OrderDetail
        {
            Order = order,
            ProductId = d.ProductId,
            UnitPrice = d.UnitPrice,
            Quantity = d.Quantity
        }).ToList();

        // 3. Agregar los detalles vinculados
        await context.AddOrderDetailsAsync(details, cancellationToken);
    }
}
