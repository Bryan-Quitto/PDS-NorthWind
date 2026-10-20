namespace NorthWind.Sales.Frontend.Views.Components;

public partial class CustomerSectionCard : ComponentBase
{
    [Parameter]
    public CreateOrderViewModel Model { get; set; } = default!;

    [Parameter]
    public EventCallback OnOpenCustomerModal { get; set; }

    [Parameter]
    public EventCallback OnClearCustomer { get; set; }

    protected bool _isClearModalOpen;

    protected void AskClearCustomer()
    {
        _isClearModalOpen = true;
    }

    protected void CancelClearCustomer()
    {
        _isClearModalOpen = false;
    }

    protected async Task ConfirmClearCustomerAsync()
    {
        _isClearModalOpen = false;
        await OnClearCustomer.InvokeAsync();
    }

    protected async Task HandleOpenCustomerModalAsync()
    {
        await OnOpenCustomerModal.InvokeAsync();
    }
}
