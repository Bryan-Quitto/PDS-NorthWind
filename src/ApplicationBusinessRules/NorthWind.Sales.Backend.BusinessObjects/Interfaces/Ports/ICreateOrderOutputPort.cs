namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface ICreateOrderOutputPort
{
    int OrderId { get; }
    Task HandleAsync(int orderId, CancellationToken cancellationToken = default);
}
