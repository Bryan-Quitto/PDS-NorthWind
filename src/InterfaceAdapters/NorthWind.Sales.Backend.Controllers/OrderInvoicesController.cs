namespace NorthWind.Sales.Backend.Controllers;

public static class OrderInvoicesController
{
    public static WebApplication UseOrderInvoicesController(this WebApplication app)
    {
        // 1. Endpoint para búsqueda paginada de órdenes
        app.MapGet(Endpoints.SearchOrders, async (
            [FromQuery] string? searchTerm,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            ISearchOrdersInputPort inputPort,
            ISearchOrdersOutputPort outputPort,
            CancellationToken cancellationToken) =>
        {
            var query = new SearchOrdersQueryDto(
                searchTerm,
                pageNumber ?? 1,
                pageSize ?? 10);

            await inputPort.HandleAsync(query, cancellationToken);
            return Results.Ok(outputPort.Result);
        })
        .WithName("SearchOrders")
        .WithTags("Orders")
        .RequireAuthorization()
        .Produces<PaginatedResultDto<OrderSummaryDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        // 2. Endpoint para obtener la factura completa por ID
        app.MapGet(Endpoints.GetOrderInvoice, async (
            [FromRoute] int id,
            IGetOrderInvoiceInputPort inputPort,
            IGetOrderInvoiceOutputPort outputPort,
            CancellationToken cancellationToken) =>
        {
            await inputPort.HandleAsync(id, cancellationToken);
            return outputPort.Result is not null
                ? Results.Ok(outputPort.Result)
                : Results.NotFound();
        })
        .WithName("GetOrderInvoice")
        .WithTags("Orders")
        .RequireAuthorization()
        .Produces<OrderInvoiceDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        // 3. Endpoint para generar y descargar el PDF de la factura
        app.MapGet(Endpoints.GetOrderPdf, async (
            [FromRoute] int id,
            IGetOrderInvoiceInputPort inputPort,
            IGetOrderInvoiceOutputPort outputPort,
            NorthWind.Sales.Backend.BusinessObjects.Interfaces.Services.IInvoicePdfGenerator pdfGenerator,
            CancellationToken cancellationToken) =>
        {
            await inputPort.HandleAsync(id, cancellationToken);
            if (outputPort.Result is null)
            {
                return Results.NotFound();
            }

            var pdfBytes = pdfGenerator.GenerateInvoicePdf(outputPort.Result);
            var fileName = $"Factura_NorthWind_{outputPort.Result.OrderId:D8}.pdf";

            return Results.File(pdfBytes, "application/pdf", fileName);
        })
        .WithName("GetOrderPdf")
        .WithTags("Orders")
        .RequireAuthorization()
        .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }
}
