namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface IGetOrderInvoiceOutputPort
{
    OrderInvoiceDto? Result { get; }
    Task HandleAsync(OrderInvoiceDto? result, CancellationToken cancellationToken = default);
}
