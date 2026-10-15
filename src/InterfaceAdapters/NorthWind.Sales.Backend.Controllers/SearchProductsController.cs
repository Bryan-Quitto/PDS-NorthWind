using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Backend.Controllers;

public static class SearchProductsController
{
    public static WebApplication UseSearchProductsController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchProducts, async (
            [FromQuery] string? searchTerm,
            [FromQuery] ProductSearchCriteria? criteria,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            ISearchProductsInputPort inputPort,
            ISearchProductsOutputPort outputPort,
            CancellationToken cancellationToken) =>
        {
            var query = new SearchProductsQueryDto(
                searchTerm,
                criteria ?? ProductSearchCriteria.All,
                pageNumber ?? 1,
                pageSize ?? 10);

            await inputPort.HandleAsync(query, cancellationToken);
            return Results.Ok(outputPort.Result);
        })
        .WithName("SearchProducts")
        .WithTags("Products")
        .RequireAuthorization()
        .Produces<PaginatedResultDto<ProductDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }
}
