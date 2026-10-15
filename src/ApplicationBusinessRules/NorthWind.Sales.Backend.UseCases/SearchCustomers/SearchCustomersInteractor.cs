using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos;

namespace NorthWind.Sales.Backend.UseCases.SearchCustomers;

public class SearchCustomersInteractor(
    IQueriesRepository queriesRepository,
    ISearchCustomersOutputPort outputPort) : ISearchCustomersInputPort
{
    public async Task HandleAsync(SearchCustomersQueryDto queryDto, CancellationToken cancellationToken = default)
    {
        var result = await queriesRepository.SearchCustomersAsync(
            queryDto.SearchTerm,
            queryDto.Criteria,
            queryDto.PageNumber,
            queryDto.PageSize,
            cancellationToken);

        await outputPort.HandleAsync(result, cancellationToken);
    }
}
