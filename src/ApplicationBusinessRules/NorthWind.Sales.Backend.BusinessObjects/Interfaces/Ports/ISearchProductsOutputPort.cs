namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface ISearchProductsOutputPort
{
    PaginatedResultDto<ProductDto> Result { get; }
    Task HandleAsync(PaginatedResultDto<ProductDto> result, CancellationToken cancellationToken = default);
}
