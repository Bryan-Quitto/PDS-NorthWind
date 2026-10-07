namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;

public interface ISearchCustomersOutputPort
{
    PaginatedResultDto<CustomerDto> Result { get; }
    Task HandleAsync(PaginatedResultDto<CustomerDto> result, CancellationToken cancellationToken = default);
}
