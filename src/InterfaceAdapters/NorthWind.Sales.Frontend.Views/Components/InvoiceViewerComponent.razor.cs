using Microsoft.JSInterop;
using NorthWind.RazorComponents.Services;

namespace NorthWind.Sales.Frontend.Views.Components;

public partial class InvoiceViewerComponent : ComponentBase
{
    [Parameter]
    public OrderInvoiceDto Invoice { get; set; } = default!;

    [Parameter]
    public EventCallback OnBackToSearch { get; set; }

    [Inject]
    public IOrderInvoiceGateway InvoiceGateway { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    public IFileSaverService FileSaverService { get; set; } = default!;

    protected bool _isDownloadingPdf;
    protected Exception? _currentException;

    protected async Task HandleBackAsync()
    {
        await OnBackToSearch.InvokeAsync();
    }

    protected async Task DownloadPdfAsync()
    {
        if (Invoice is null) return;

        _isDownloadingPdf = true;
        _currentException = null;

        try
        {
            var pdfBytes = await InvoiceGateway.GetOrderPdfAsync(Invoice.OrderId);
            var fileName = $"Factura_NorthWind_{Invoice.OrderId:D8}.pdf";

            await FileSaverService.SaveFileAsync(fileName, "application/pdf", pdfBytes);
        }
        catch (Exception ex)
        {
            _currentException = ex;
        }
        finally
        {
            _isDownloadingPdf = false;
        }
    }
}
