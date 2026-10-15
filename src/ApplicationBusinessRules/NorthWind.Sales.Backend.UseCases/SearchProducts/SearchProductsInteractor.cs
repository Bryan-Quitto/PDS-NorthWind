using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos;

namespace NorthWind.Sales.Backend.UseCases.SearchProducts;

public class SearchProductsInteractor(
    IQueriesRepository queriesRepository,
    ISearchProductsOutputPort outputPort) : ISearchProductsInputPort
{
    public async Task HandleAsync(SearchProductsQueryDto queryDto, CancellationToken cancellationToken = default)
    {
        var result = await queriesRepository.SearchProductsAsync(
            queryDto.SearchTerm,
            queryDto.Criteria,
            queryDto.PageNumber,
            queryDto.PageSize,
            cancellationToken);

        await outputPort.HandleAsync(result, cancellationToken);
    }
}
