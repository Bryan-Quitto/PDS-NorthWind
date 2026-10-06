namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface ICreateOrderInputPort
{
    Task HandleAsync(CreateOrderDto orderDto, CancellationToken cancellationToken = default);
}
