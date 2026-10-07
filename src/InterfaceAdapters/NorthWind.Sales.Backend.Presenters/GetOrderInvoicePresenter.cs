using Microsoft.AspNetCore.Http;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.Entities.Dtos;

namespace NorthWind.Sales.Backend.Presenters;

public class GetOrderInvoicePresenter : IGetOrderInvoiceOutputPort
{
    public OrderInvoiceDto? Result { get; private set; }

    public Task HandleAsync(OrderInvoiceDto? result, CancellationToken cancellationToken = default)
    {
        Result = result;
        return Task.CompletedTask;
    }

    public IResult ToResult() => Result is null ? Results.NotFound() : Results.Ok(Result);
}
