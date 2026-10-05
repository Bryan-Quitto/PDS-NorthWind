using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.Entities.Dtos;
using NorthWind.Sales.Entities.ValueObjects;

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
        .Produces<int>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }
}
