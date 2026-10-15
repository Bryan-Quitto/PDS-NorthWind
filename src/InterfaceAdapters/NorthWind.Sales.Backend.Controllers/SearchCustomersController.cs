using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Backend.Controllers;

public static class SearchCustomersController
{
    public static WebApplication UseSearchCustomersController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchCustomers, async (
            [FromQuery] string? searchTerm,
            [FromQuery] CustomerSearchCriteria? criteria,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            ISearchCustomersInputPort inputPort,
            ISearchCustomersOutputPort outputPort,
            CancellationToken cancellationToken) =>
        {
            var query = new SearchCustomersQueryDto(
                searchTerm,
                criteria ?? CustomerSearchCriteria.All,
                pageNumber ?? 1,
                pageSize ?? 10);

            await inputPort.HandleAsync(query, cancellationToken);
            return Results.Ok(outputPort.Result);
        })
        .WithName("SearchCustomers")
        .WithTags("Customers")
        .RequireAuthorization()
        .Produces<PaginatedResultDto<CustomerDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }
}
