namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface ISearchOrdersOutputPort
{
    PaginatedResultDto<OrderSummaryDto> Result { get; }
    Task HandleAsync(PaginatedResultDto<OrderSummaryDto> result, CancellationToken cancellationToken = default);
}
