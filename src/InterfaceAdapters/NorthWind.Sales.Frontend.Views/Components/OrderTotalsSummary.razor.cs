namespace NorthWind.Sales.Frontend.Views.Components;

public partial class OrderTotalsSummary : ComponentBase
{
    [Parameter]
    public decimal Subtotal { get; set; }

    [Parameter]
    public decimal Tax { get; set; }

    [Parameter]
    public decimal Total { get; set; }

    [Parameter]
    public bool IsSubmitting { get; set; }

    [Parameter]
    public bool CanSubmit { get; set; }

    [Parameter]
    public EventCallback OnOpenClearOrderModal { get; set; }

    protected async Task HandleOpenClearOrderModalAsync()
    {
        await OnOpenClearOrderModal.InvokeAsync();
    }
}
