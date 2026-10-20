namespace NorthWind.RazorComponents;

/// <summary>
/// Componente reutilizable de barra de búsqueda con selector de criterios, botón de limpiar y soporte para tecla Enter.
/// </summary>
public partial class CriteriaSearchBar : ComponentBase
{
    private bool _isDropdownOpen;

    /// <summary>
    /// Lista de criterios de búsqueda disponibles.
    /// </summary>
    [Parameter]
    public IReadOnlyList<SearchCriterion> Criteria { get; set; } = [];

    /// <summary>
    /// Clave técnica del criterio actualmente seleccionado.
    /// Soporta two-way binding con @bind-SelectedCriterionKey.
    /// </summary>
    [Parameter]
    public string? SelectedCriterionKey { get; set; }

    /// <summary>
    /// Evento emitido al cambiar el criterio de búsqueda seleccionado.
    /// </summary>
    [Parameter]
    public EventCallback<string> SelectedCriterionKeyChanged { get; set; }

    /// <summary>
    /// Texto actual ingresado en la caja de búsqueda.
    /// Soporta two-way binding con @bind-SearchText.
    /// </summary>
    [Parameter]
    public string? SearchText { get; set; }

    /// <summary>
    /// Evento emitido cuando el texto de búsqueda cambia.
    /// </summary>
    [Parameter]
    public EventCallback<string> SearchTextChanged { get; set; }

    /// <summary>
    /// Texto de sugerencia (placeholder) en el campo de entrada.
    /// </summary>
    [Parameter]
    public string Placeholder { get; set; } = "Buscar...";

    /// <summary>
    /// Longitud máxima permitida para el texto de búsqueda.
    /// </summary>
    [Parameter]
    public int? MaxLength { get; set; }

    /// <summary>
    /// Evento emitido cuando el usuario presiona el botón de búsqueda o la tecla Enter.
    /// </summary>
    [Parameter]
    public EventCallback OnSearch { get; set; }

    /// <summary>
    /// Evento emitido cuando el usuario limpia la búsqueda.
    /// </summary>
    [Parameter]
    public EventCallback OnClear { get; set; }

    /// <summary>
    /// Indica si se está ejecutando una búsqueda en segundo plano para deshabilitar controles y mostrar indicador.
    /// </summary>
    [Parameter]
    public bool IsSearching { get; set; }

    /// <summary>
    /// Texto a mostrar en el botón principal de búsqueda.
    /// </summary>
    [Parameter]
    public string SearchButtonText { get; set; } = "Buscar";

    /// <summary>
    /// Texto del botón de limpiar o tooltip del mismo.
    /// </summary>
    [Parameter]
    public string ClearButtonText { get; set; } = "Limpiar";

    /// <summary>
    /// Determina si se muestra el botón de limpiar cuando hay texto escrito.
    /// </summary>
    [Parameter]
    public bool ShowClearButton { get; set; } = true;

    /// <summary>
    /// Clases CSS adicionales para el contenedor principal de la barra de búsqueda.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Criterio actualmente activo.
    /// </summary>
    public SearchCriterion? SelectedCriterion =>
        Criteria.FirstOrDefault(c => string.Equals(c.Key, SelectedCriterionKey, StringComparison.OrdinalIgnoreCase))
        ?? Criteria.FirstOrDefault();

    /// <summary>
    /// Alterna la visibilidad del menú desplegable de criterios.
    /// </summary>
    protected void ToggleDropdown()
    {
        if (!IsSearching)
        {
            _isDropdownOpen = !_isDropdownOpen;
        }
    }

    /// <summary>
    /// Selecciona un nuevo criterio y cierra el menú desplegable.
    /// </summary>
    protected async Task SelectCriterionAsync(string key)
    {
        SelectedCriterionKey = key;
        _isDropdownOpen = false;
        await SelectedCriterionKeyChanged.InvokeAsync(key);
    }

    /// <summary>
    /// Maneja los cambios de texto en tiempo real.
    /// </summary>
    protected async Task HandleSearchInputAsync(ChangeEventArgs e)
    {
        SearchText = e.Value?.ToString() ?? string.Empty;
        await SearchTextChanged.InvokeAsync(SearchText);
    }

    /// <summary>
    /// Maneja las pulsaciones de teclado como Enter (buscar) y Escape (cerrar menú).
    /// </summary>
    protected async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await HandleSearchAsync();
        }
        else if (e.Key == "Escape" && _isDropdownOpen)
        {
            _isDropdownOpen = false;
        }
    }

    /// <summary>
    /// Dispara la acción de búsqueda si no está en proceso.
    /// </summary>
    protected async Task HandleSearchAsync()
    {
        if (IsSearching) return;

        _isDropdownOpen = false;
        await OnSearch.InvokeAsync();
    }

    /// <summary>
    /// Limpia el campo de texto y dispara la acción de limpieza.
    /// </summary>
    protected async Task HandleClearAsync()
    {
        if (IsSearching) return;

        SearchText = string.Empty;
        _isDropdownOpen = false;
        await SearchTextChanged.InvokeAsync(string.Empty);
        await OnClear.InvokeAsync();
    }
}
