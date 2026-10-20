namespace NorthWind.Sales.Frontend.Views;

public partial class CreateOrderComponent : ComponentBase
{
    [Parameter]
    public CreateOrderViewModel Model { get; set; } = new();

    [Inject]
    public ICreateOrderGateway OrderGateway { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    protected EditContext _editContext = default!;
    protected ValidationMessageStore _serverMessageStore = default!;
    protected Exception? _currentException;
    protected int? _createdOrderId;
    protected bool _isSubmitting;

    protected bool _isCustomerModalOpen;
    protected bool _isProductModalOpen;
    protected bool _isClearProductsModalOpen;
    protected bool _isClearOrderModalOpen;
    protected int? _replacingProductId;

    protected override void OnInitialized()
    {
        InitializeEditContext();
    }

    protected void InitializeEditContext()
    {
        _editContext = new EditContext(Model);
        _serverMessageStore = new ValidationMessageStore(_editContext);
        _editContext.OnFieldChanged += (s, e) =>
        {
            _serverMessageStore.Clear(e.FieldIdentifier);
            _editContext.NotifyValidationStateChanged();
        };
    }

    protected void ClearServerErrors()
    {
        _currentException = null;
        _serverMessageStore.Clear();
    }

    protected void OpenCustomerModal()
    {
        _isCustomerModalOpen = true;
    }

    protected void HandleCustomerSelected(CustomerDto customer)
    {
        ClearServerErrors();
        Model.SetCustomer(customer);
        _editContext.NotifyValidationStateChanged();
        StateHasChanged();
    }

    protected void HandleClearCustomer()
    {
        ClearServerErrors();
        Model.ClearCustomer();
        _editContext.NotifyValidationStateChanged();
        StateHasChanged();
    }

    protected void OpenProductModal()
    {
        ClearServerErrors();
        _replacingProductId = null;
        _isProductModalOpen = true;
    }

    protected void OpenReplaceProductModal(int productId)
    {
        ClearServerErrors();
        _replacingProductId = productId;
        _isProductModalOpen = true;
    }

    protected void HandleProductSelected(ProductDto product)
    {
        ClearServerErrors();
        if (_replacingProductId.HasValue)
        {
            Model.ReplaceDetail(_replacingProductId.Value, product.Id, product.Name, product.UnitPrice, product.UnitsInStock);
            _replacingProductId = null;
        }
        else
        {
            Model.AddOrUpdateDetail(product.Id, product.Name, product.UnitPrice, 1, product.UnitsInStock);
        }

        _editContext.NotifyValidationStateChanged();
        StateHasChanged();
    }

    protected void HandleQuantityChanged((CreateOrderDetailViewModel Detail, ChangeEventArgs Args) e)
    {
        ClearServerErrors();
        var rawVal = e.Args.Value?.ToString() ?? string.Empty;

        if (int.TryParse(rawVal, out var intQty) && intQty > 0 && intQty <= short.MaxValue)
        {
            Model.UpdateQuantity(e.Detail, (short)intQty, rawVal);
        }
        else
        {
            Model.UpdateQuantity(e.Detail, 0, rawVal);
        }

        _editContext.NotifyFieldChanged(new FieldIdentifier(e.Detail, nameof(e.Detail.RawQuantity)));
        _editContext.NotifyFieldChanged(new FieldIdentifier(e.Detail, nameof(e.Detail.Quantity)));
        StateHasChanged();
    }

    protected void HandleRemoveProduct(CreateOrderDetailViewModel detail)
    {
        ClearServerErrors();
        Model.RemoveDetail(detail);
        _editContext.NotifyValidationStateChanged();
        StateHasChanged();
    }

    protected void OpenClearProductsModal()
    {
        _isClearProductsModalOpen = true;
    }

    protected void CloseClearProductsModal()
    {
        _isClearProductsModalOpen = false;
    }

    protected void ConfirmClearProducts()
    {
        ClearServerErrors();
        Model.OrderDetails.Clear();
        _replacingProductId = null;
        _isClearProductsModalOpen = false;
        _editContext.NotifyValidationStateChanged();
        StateHasChanged();
    }

    protected void OpenClearOrderModal()
    {
        _isClearOrderModalOpen = true;
    }

    protected void CloseClearOrderModal()
    {
        _isClearOrderModalOpen = false;
    }

    protected void ConfirmClearOrder()
    {
        ClearServerErrors();
        Model.Reset();
        _replacingProductId = null;
        _isClearOrderModalOpen = false;
        InitializeEditContext();
        StateHasChanged();
    }

    protected async Task HandleValidSubmit()
    {
        _currentException = null;
        _serverMessageStore.Clear();
        _editContext.NotifyValidationStateChanged();
        _isSubmitting = true;

        try
        {
            var dto = Model.ToDto();
            var orderId = await OrderGateway.CreateOrderAsync(dto);
            NavigationManager.NavigateTo($"invoices/{orderId}");
        }
        catch (Exception ex)
        {
            _currentException = ex;

            // Inyectar errores devueltos por el backend al ValidationMessageStore del formulario
            if (ex.Data.Contains("Errors") && ex.Data["Errors"] is IEnumerable<ValidationError> validationErrors)
            {
                var errorsList = validationErrors.ToList();
                if (errorsList.Count > 0)
                {
                    foreach (var error in errorsList)
                    {
                        var fieldIdentifier = ModelValidator<CreateOrderViewModel>.ResolveFieldIdentifier(Model, error.PropertyName);
                        var fieldMessage = error.ErrorMessage.Contains("superan las existencias", StringComparison.OrdinalIgnoreCase)
                            ? "Stock insuficiente"
                            : error.ErrorMessage;

                        _serverMessageStore.Add(fieldIdentifier, fieldMessage);
                    }
                    _editContext.NotifyValidationStateChanged();
                }
            }
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    protected void ResetForm()
    {
        _createdOrderId = null;
        _currentException = null;
        Model.Reset();
        InitializeEditContext();
    }
}
