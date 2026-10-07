using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos;

namespace NorthWind.Sales.Backend.UseCases.GetOrderInvoice;

public class GetOrderInvoiceInteractor(
    IQueriesRepository queriesRepository,
    IGetOrderInvoiceOutputPort outputPort) : IGetOrderInvoiceInputPort
{
    public async Task HandleAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var result = await queriesRepository.GetOrderInvoiceByIdAsync(orderId, cancellationToken);
        await outputPort.HandleAsync(result, cancellationToken);
    }
}
