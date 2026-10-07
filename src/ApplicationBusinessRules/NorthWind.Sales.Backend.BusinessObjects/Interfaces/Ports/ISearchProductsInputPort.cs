namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface ISearchProductsInputPort
{
    Task HandleAsync(SearchProductsQueryDto queryDto, CancellationToken cancellationToken = default);
}
