namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface ISearchCustomersInputPort
{
    Task HandleAsync(SearchCustomersQueryDto queryDto, CancellationToken cancellationToken = default);
}
