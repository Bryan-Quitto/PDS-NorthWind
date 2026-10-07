namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface ISearchOrdersInputPort
{
    Task HandleAsync(SearchOrdersQueryDto queryDto, CancellationToken cancellationToken = default);
}
