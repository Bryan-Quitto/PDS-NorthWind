namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface IGetOrderInvoiceInputPort
{
    Task HandleAsync(int orderId, CancellationToken cancellationToken = default);
}
