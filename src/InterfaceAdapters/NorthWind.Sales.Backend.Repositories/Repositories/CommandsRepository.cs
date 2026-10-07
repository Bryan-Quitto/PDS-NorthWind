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

    public async Task<IReadOnlyList<TrackedProduct>> GetProductsByIdsAsync(
        IEnumerable<int> ids,
        CancellationToken cancellationToken = default)
    {
        var products = await context.GetProductsByIdsAsync(ids, cancellationToken);

        var trackedProducts = products.Select(p => new TrackedProduct
        {
            Id = p.Id,
            Name = p.Name,
            UnitPrice = p.UnitPrice,
            UnitsInStock = p.UnitsInStock,
            RowVersion = p.RowVersion
        }).ToList();

        return trackedProducts;
    }

    public void UpdateProducts(IEnumerable<TrackedProduct> products)
    {
        var updatedEntities = products.Select(tp => new Entities.Product
        {
            Id = tp.Id,
            Name = tp.Name,
            UnitPrice = tp.UnitPrice,
            UnitsInStock = tp.UnitsInStock,
            RowVersion = tp.RowVersion
        }).ToList();

        context.UpdateProducts(updatedEntities);
    }
}
