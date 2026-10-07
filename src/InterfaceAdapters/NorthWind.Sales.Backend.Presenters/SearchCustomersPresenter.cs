using Microsoft.AspNetCore.Http;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.Entities.Dtos;

namespace NorthWind.Sales.Backend.Presenters;

public class SearchCustomersPresenter : ISearchCustomersOutputPort
{
    public PaginatedResultDto<CustomerDto> Result { get; private set; } = new([], 0, 1, 10);

    public Task HandleAsync(PaginatedResultDto<CustomerDto> result, CancellationToken cancellationToken = default)
    {
        Result = result;
        return Task.CompletedTask;
    }

    public IResult ToResult() => Results.Ok(Result);
}
