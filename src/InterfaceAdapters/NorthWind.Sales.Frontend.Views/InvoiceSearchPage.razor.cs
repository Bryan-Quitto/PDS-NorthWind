namespace NorthWind.Sales.Frontend.Views;

public partial class InvoiceSearchPage : ComponentBase, IDisposable
{
    private CancellationTokenSource? _searchCts;
    [Parameter]
    public int? OrderId { get; set; }

    [Inject]
    public IOrderInvoiceGateway InvoiceGateway { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    protected OrderSearchCriteria _selectedCriteria = OrderSearchCriteria.All;
    protected string _searchTerm = string.Empty;
    protected int _currentPage = 1;
    protected const int PageSize = 10;
    protected int _totalCount = 0;
    protected int _totalPages = 1;
    protected bool _isLoading = false;
    protected bool _isLoadingInvoice = false;
    protected Exception? _currentException;

    protected IReadOnlyList<OrderSummaryDto> _orders = [];
    protected OrderInvoiceDto? _selectedInvoice;

    protected static readonly IReadOnlyList<SearchCriterion> OrderCriteria =
    [
        new(OrderSearchCriteria.All.ToString(), ViewMessages.CriteriaAll, "bi bi-grid-fill"),
        new(OrderSearchCriteria.OrderId.ToString(), ViewMessages.CriteriaInvoiceId, "bi bi-receipt"),
        new(OrderSearchCriteria.CustomerName.ToString(), ViewMessages.CriteriaName, "bi bi-person-badge"),
        new(OrderSearchCriteria.CustomerIdentification.ToString(), ViewMessages.CriteriaIdentification, "bi bi-card-text"),
        new(OrderSearchCriteria.CustomerId.ToString(), ViewMessages.CriteriaCustomerId, "bi bi-key-fill")
    ];

    protected override async Task OnInitializedAsync()
    {
        await SearchOrdersAsync(1);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (OrderId.HasValue && OrderId.Value > 0)
        {
            await LoadInvoiceAsync(OrderId.Value);
        }
    }

    protected async Task HandleCriterionChangedAsync(string key)
    {
        if (Enum.TryParse<OrderSearchCriteria>(key, out var criteria))
        {
            _selectedCriteria = criteria;
            if (string.IsNullOrEmpty(_searchTerm))
            {
                await SearchOrdersAsync(1);
            }
        }
    }

    protected string GetSearchPlaceholder() => _selectedCriteria switch
    {
        OrderSearchCriteria.OrderId => "Buscar por No. de Factura (ej. 10248)...",
        OrderSearchCriteria.CustomerName => "Buscar por Nombre o Razón Social de cliente...",
        OrderSearchCriteria.CustomerIdentification => "Buscar por Cédula o RUC del cliente...",
        OrderSearchCriteria.CustomerId => "Buscar por Código ID de cliente (ej. ALFKI)...",
        _ => "Buscar por No. Factura, Nombre, Cédula o ID de Cliente..."
    };

    protected int? GetSearchMaxLength() => _selectedCriteria switch
    {
        OrderSearchCriteria.OrderId => 10,
        OrderSearchCriteria.CustomerIdentification => 10,
        OrderSearchCriteria.CustomerId => 5,
        OrderSearchCriteria.CustomerName => 50,
        _ => 50
    };

    public async Task SearchOrdersAsync(int pageNumber)
    {
        _searchCts?.Cancel();
        _searchCts?.Dispose();
        _searchCts = new CancellationTokenSource();
        var cancellationToken = _searchCts.Token;

        _isLoading = true;
        _currentException = null;
        _currentPage = pageNumber;

        try
        {
            var term = _selectedCriteria == OrderSearchCriteria.CustomerId
                ? _searchTerm.ToUpperInvariant()
                : _searchTerm;

            var result = await InvoiceGateway.SearchOrdersAsync(term, _selectedCriteria, _currentPage, PageSize, cancellationToken);
            _orders = result.Items;
            _totalCount = result.TotalCount;
            _totalPages = (int)Math.Ceiling((double)_totalCount / PageSize);
            if (_totalPages < 1) _totalPages = 1;
        }
        catch (OperationCanceledException)
        {
            // Búsqueda cancelada por una solicitud más reciente; ignorar limpiamente sin mostrar error ni romper la UI
        }
        catch (Exception ex)
        {
            _currentException = ex;
        }
        finally
        {
            if (_searchCts?.Token == cancellationToken)
            {
                _isLoading = false;
            }
        }
    }

    public void Dispose()
    {
        _searchCts?.Cancel();
        _searchCts?.Dispose();
        _searchCts = null;
    }

    protected async Task ClearSearchAsync()
    {
        _searchTerm = string.Empty;
        await SearchOrdersAsync(1);
    }

    public async Task LoadInvoiceAsync(int orderId)
    {
        _isLoadingInvoice = true;
        _currentException = null;

        try
        {
            _selectedInvoice = await InvoiceGateway.GetOrderInvoiceAsync(orderId);
            if (_selectedInvoice is null)
            {
                _currentException = new InvalidOperationException(ViewMessages.InvoiceNotFound);
            }
        }
        catch (Exception ex)
        {
            _currentException = ex;
        }
        finally
        {
            _isLoadingInvoice = false;
        }
    }

    protected void ClearSelectedInvoice()
    {
        _selectedInvoice = null;
        _currentException = null;
        if (OrderId.HasValue)
        {
            OrderId = null;
            NavigationManager.NavigateTo("invoices");
        }
    }
}
