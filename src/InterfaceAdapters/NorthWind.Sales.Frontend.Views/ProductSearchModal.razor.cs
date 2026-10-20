namespace NorthWind.Sales.Frontend.Views;

public partial class ProductSearchModal : ComponentBase
{
    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    [Parameter]
    public EventCallback<ProductDto> OnProductSelected { get; set; }

    [Parameter]
    public int? ExcludedProductId { get; set; }

    [Inject]
    public ISearchGateway SearchGateway { get; set; } = default!;

    protected ProductSearchCriteria _selectedCriteria = ProductSearchCriteria.All;
    protected string _searchTerm = string.Empty;
    protected int _currentPage = 1;
    protected const int PageSize = 10;
    protected int _totalCount = 0;
    protected int _totalPages = 1;
    protected bool _isLoading = false;
    protected IReadOnlyList<ProductDto> _products = [];

    protected static readonly IReadOnlyList<SearchCriterion> ProductCriteria =
    [
        new(ProductSearchCriteria.All.ToString(), ViewMessages.CriteriaAll, "bi bi-grid-fill"),
        new(ProductSearchCriteria.Name.ToString(), ViewMessages.CriteriaProductName, "bi bi-tag-fill"),
        new(ProductSearchCriteria.Id.ToString(), ViewMessages.CriteriaProductId, "bi bi-hash")
    ];

    protected override async Task OnParametersSetAsync()
    {
        if (IsOpen && _products.Count == 0 && !_isLoading)
        {
            await SearchAsync(1);
        }
    }

    protected async Task HandleCriterionChangedAsync(string key)
    {
        if (Enum.TryParse<ProductSearchCriteria>(key, out var criteria))
        {
            _selectedCriteria = criteria;
            _searchTerm = string.Empty;
            await SearchAsync(1);
        }
    }

    protected string GetSearchPlaceholder() => _selectedCriteria switch
    {
        ProductSearchCriteria.Name => "Buscar por nombre del producto (ej. Chai, Queso)...",
        ProductSearchCriteria.Id => "Buscar por ID numérico de producto (ej. 1, 10)...",
        _ => "Buscar por ID o nombre de producto..."
    };

    protected int? GetSearchMaxLength() => _selectedCriteria switch
    {
        ProductSearchCriteria.Id => 10,
        ProductSearchCriteria.Name => 40,
        _ => 40
    };

    public async Task SearchAsync(int pageNumber)
    {
        _isLoading = true;
        _currentPage = pageNumber;

        try
        {
            var result = await SearchGateway.SearchProductsAsync(_searchTerm, _selectedCriteria, _currentPage, PageSize);
            _products = result.Items;
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

    protected async Task SelectProductAsync(ProductDto product)
    {
        await OnProductSelected.InvokeAsync(product);
        await CloseAsync();
    }

    protected async Task CloseAsync()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(false);
    }
}
