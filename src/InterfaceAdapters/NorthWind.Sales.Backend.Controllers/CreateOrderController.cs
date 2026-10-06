namespace NorthWind.Sales.Backend.Controllers;

public static class CreateOrderController
{
    public static WebApplication UseCreateOrderController(this WebApplication app)
    {
        app.MapPost(Endpoints.CreateOrder, async (
            [FromBody] CreateOrderDto orderDto,
            ICreateOrderInputPort inputPort,
            ICreateOrderOutputPort outputPort,
            CancellationToken cancellationToken) =>
        {
            await inputPort.HandleAsync(orderDto, cancellationToken);
            return Results.Ok(outputPort.OrderId);
        })
        .WithName("CreateOrder")
        .WithTags("Orders")
        .RequireAuthorization()
        .Produces<int>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }
}
