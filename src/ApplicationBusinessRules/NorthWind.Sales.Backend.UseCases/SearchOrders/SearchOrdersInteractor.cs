using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos;

namespace NorthWind.Sales.Backend.UseCases.SearchOrders;

public class SearchOrdersInteractor(
    IQueriesRepository queriesRepository,
    ISearchOrdersOutputPort outputPort) : ISearchOrdersInputPort
{
    public async Task HandleAsync(SearchOrdersQueryDto queryDto, CancellationToken cancellationToken = default)
    {
        var result = await queriesRepository.SearchOrdersAsync(
            queryDto.SearchTerm,
            queryDto.PageNumber,
            queryDto.PageSize,
            cancellationToken);

        await outputPort.HandleAsync(result, cancellationToken);
    }
}
