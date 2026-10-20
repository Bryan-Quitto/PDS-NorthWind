namespace NorthWind.Sales.Frontend.Views.Components;

public partial class OrderItemsTable : ComponentBase
{
    [Parameter]
    public IList<CreateOrderDetailViewModel> Items { get; set; } = [];

    [Parameter]
    public EventCallback OnOpenAddProductModal { get; set; }

    [Parameter]
    public EventCallback OnOpenClearProductsModal { get; set; }

    [Parameter]
    public EventCallback<int> OnReplaceProduct { get; set; }

    [Parameter]
    public EventCallback<CreateOrderDetailViewModel> OnRemoveProduct { get; set; }

    [Parameter]
    public EventCallback<(CreateOrderDetailViewModel Detail, ChangeEventArgs Args)> OnQuantityChanged { get; set; }

    protected bool _isDeleteProductModalOpen;
    protected CreateOrderDetailViewModel? _detailToDelete;

    protected async Task HandleOpenAddProductModalAsync()
    {
        await OnOpenAddProductModal.InvokeAsync();
    }

    protected async Task HandleOpenClearProductsModalAsync()
    {
        await OnOpenClearProductsModal.InvokeAsync();
    }

    protected async Task HandleReplaceProductAsync(int productId)
    {
        await OnReplaceProduct.InvokeAsync(productId);
    }

    protected void AskRemoveDetail(CreateOrderDetailViewModel detail)
    {
        _detailToDelete = detail;
        _isDeleteProductModalOpen = true;
    }

    protected void CancelRemoveDetail()
    {
        _isDeleteProductModalOpen = false;
        _detailToDelete = null;
    }

    protected async Task ConfirmRemoveDetailAsync()
    {
        _isDeleteProductModalOpen = false;
        if (_detailToDelete != null)
        {
            var detail = _detailToDelete;
            _detailToDelete = null;
            await OnRemoveProduct.InvokeAsync(detail);
        }
    }

    protected async Task HandleQuantityChangedAsync(CreateOrderDetailViewModel detail, ChangeEventArgs e)
    {
        await OnQuantityChanged.InvokeAsync((detail, e));
    }
}
