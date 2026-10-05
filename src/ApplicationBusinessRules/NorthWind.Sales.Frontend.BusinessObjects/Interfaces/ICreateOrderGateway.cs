namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ICreateOrderGateway
{
    Task<int> CreateOrderAsync(CreateOrderDto orderDto, CancellationToken cancellationToken = default);
}
