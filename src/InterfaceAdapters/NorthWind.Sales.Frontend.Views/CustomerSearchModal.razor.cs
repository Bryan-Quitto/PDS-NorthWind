namespace NorthWind.Sales.Frontend.Views;

public partial class CustomerSearchModal : ComponentBase
{
    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    [Parameter]
    public EventCallback<CustomerDto> OnCustomerSelected { get; set; }

    [Inject]
    public ISearchGateway SearchGateway { get; set; } = default!;

    protected CustomerSearchCriteria _selectedCriteria = CustomerSearchCriteria.All;
    protected string _searchTerm = string.Empty;
    protected int _currentPage = 1;
    protected const int PageSize = 5;
    protected int _totalCount = 0;
    protected int _totalPages = 1;
    protected bool _isLoading = false;
    protected IReadOnlyList<CustomerDto> _customers = [];

    protected static readonly IReadOnlyList<SearchCriterion> CustomerCriteria =
    [
        new(CustomerSearchCriteria.All.ToString(), ViewMessages.CriteriaAll, "bi bi-grid-fill"),
        new(CustomerSearchCriteria.Identification.ToString(), ViewMessages.CriteriaIdentification, "bi bi-card-text"),
        new(CustomerSearchCriteria.Id.ToString(), ViewMessages.CriteriaCustomerId, "bi bi-key-fill"),
        new(CustomerSearchCriteria.Name.ToString(), ViewMessages.CriteriaFullName, "bi bi-person-badge"),
        new(CustomerSearchCriteria.LastName.ToString(), ViewMessages.CriteriaLastName, "bi bi-person"),
        new(CustomerSearchCriteria.BusinessName.ToString(), ViewMessages.CriteriaBusinessName, "bi bi-building"),
        new(CustomerSearchCriteria.TradeName.ToString(), ViewMessages.CriteriaTradeName, "bi bi-shop")
    ];

    protected override async Task OnParametersSetAsync()
    {
        if (IsOpen && _customers.Count == 0 && !_isLoading)
        {
            await SearchAsync(1);
        }
    }

    protected async Task HandleCriterionChangedAsync(string key)
    {
        if (Enum.TryParse<CustomerSearchCriteria>(key, out var criteria))
        {
            _selectedCriteria = criteria;
            _searchTerm = string.Empty;
            await SearchAsync(1);
        }
    }

    protected string GetSearchPlaceholder() => _selectedCriteria switch
    {
        CustomerSearchCriteria.Identification => "Buscar por cédula (10 dígitos) o RUC (13 dígitos)...",
        CustomerSearchCriteria.Name => "Buscar por nombres y apellidos (ej. Ana Trujillo)...",
        CustomerSearchCriteria.LastName => "Buscar por primer apellido...",
        CustomerSearchCriteria.BusinessName => "Buscar por razón social de la empresa...",
        CustomerSearchCriteria.TradeName => "Buscar por nombre comercial (ej. Taquería El Paso)...",
        CustomerSearchCriteria.Id => "Buscar por código de cliente (ej. ALFKI)...",
        _ => "Buscar cliente por ID, nombres, razón social o identificación..."
    };

    protected int? GetSearchMaxLength() => _selectedCriteria switch
    {
        CustomerSearchCriteria.Identification => 13,
        CustomerSearchCriteria.Id => 5,
        CustomerSearchCriteria.LastName => 50,
        CustomerSearchCriteria.BusinessName => 150,
        CustomerSearchCriteria.TradeName => 150,
        CustomerSearchCriteria.Name => 50,
        _ => 50
    };

    public async Task SearchAsync(int pageNumber)
    {
        _isLoading = true;
        _currentPage = pageNumber;

        try
        {
            var term = _selectedCriteria == CustomerSearchCriteria.Id
                ? _searchTerm.ToUpperInvariant()
                : _searchTerm;

            var result = await SearchGateway.SearchCustomersAsync(term, _selectedCriteria, _currentPage, PageSize);
            _customers = result.Items;
            _totalCount = result.TotalCount;
            _totalPages = (int)Math.Ceiling((double)_totalCount / PageSize);
            if (_totalPages < 1) _totalPages = 1;
        }
        finally
        {
            _isLoading = false;
        }
    }

    protected async Task ClearSearchAsync()
    {
        _searchTerm = string.Empty;
        await SearchAsync(1);
    }

    protected async Task SelectCustomerAsync(CustomerDto customer)
    {
        await OnCustomerSelected.InvokeAsync(customer);
        await CloseAsync();
    }

    protected async Task CloseAsync()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(false);
    }
}
