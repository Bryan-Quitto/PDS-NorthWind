namespace NorthWind.Sales.Frontend.Views.Components;

public partial class ConfirmActionDialog : ComponentBase
{
    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    [Parameter]
    public string Title { get; set; } = "Confirmación";

    [Parameter]
    public string Message { get; set; } = string.Empty;

    [Parameter]
    public string HeaderCssClass { get; set; } = "bg-warning text-dark";

    [Parameter]
    public string TitleTextCssClass { get; set; } = string.Empty;

    [Parameter]
    public string Icon { get; set; } = "bi bi-exclamation-triangle-fill";

    [Parameter]
    public string ConfirmButtonText { get; set; } = "Confirmar";

    [Parameter]
    public string? ConfirmButtonIcon { get; set; }

    [Parameter]
    public string ConfirmButtonCssClass { get; set; } = "btn-warning";

    [Parameter]
    public string CancelButtonText { get; set; } = "Cancelar";

    [Parameter]
    public EventCallback OnConfirm { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    protected async Task HandleConfirmAsync()
    {
        await OnConfirm.InvokeAsync();
    }

    protected async Task HandleCancelAsync()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(false);
        await OnCancel.InvokeAsync();
    }
}
