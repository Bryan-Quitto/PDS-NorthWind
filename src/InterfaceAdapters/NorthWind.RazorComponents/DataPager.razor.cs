namespace NorthWind.RazorComponents;

/// <summary>
/// Componente de paginación accesible y desacoplado basado en Bootstrap 5.
/// </summary>
public partial class DataPager : ComponentBase
{
    /// <summary>
    /// Página actual activa (1-based index).
    /// </summary>
    [Parameter]
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// Cantidad total de páginas disponibles.
    /// </summary>
    [Parameter]
    public int TotalPages { get; set; } = 1;

    /// <summary>
    /// Conteo total de registros (opcional, utilizado para el texto informativo).
    /// </summary>
    [Parameter]
    public int TotalCount { get; set; }

    /// <summary>
    /// Rango de páginas visibles en la botonera numerada (por defecto 5).
    /// </summary>
    [Parameter]
    public int VisiblePageRange { get; set; } = 5;

    /// <summary>
    /// Evento emitido cuando el usuario selecciona una página diferente.
    /// </summary>
    [Parameter]
    public EventCallback<int> OnPageChanged { get; set; }

    /// <summary>
    /// Determina si se muestran los botones de ir a la primera y a la última página.
    /// </summary>
    [Parameter]
    public bool ShowFirstLastButtons { get; set; } = true;

    /// <summary>
    /// Determina si se muestra información textual del estado de paginación.
    /// </summary>
    [Parameter]
    public bool ShowPageInfo { get; set; } = false;

    /// <summary>
    /// Texto de información personalizado que reemplaza al texto generado por defecto.
    /// </summary>
    [Parameter]
    public string? CustomInfoText { get; set; }

    /// <summary>
    /// Clases CSS adicionales para el contenedor principal.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Indica si existe una página anterior disponible.
    /// </summary>
    protected bool HasPrevious => CurrentPage > 1;

    /// <summary>
    /// Indica si existe una página posterior disponible.
    /// </summary>
    protected bool HasNext => CurrentPage < TotalPages;

    /// <summary>
    /// Calcula la primera página del bloque visible.
    /// </summary>
    protected int CalculatedStartPage
    {
        get
        {
            int effectiveTotal = Math.Max(1, TotalPages);
            if (effectiveTotal <= VisiblePageRange) return 1;

            int half = VisiblePageRange / 2;
            int start = Math.Max(1, CurrentPage - half);

            if (start + VisiblePageRange - 1 > effectiveTotal)
            {
                start = Math.Max(1, effectiveTotal - VisiblePageRange + 1);
            }

            return start;
        }
    }

    /// <summary>
    /// Calcula la última página del bloque visible.
    /// </summary>
    protected int CalculatedEndPage
    {
        get
        {
            int effectiveTotal = Math.Max(1, TotalPages);
            if (effectiveTotal <= VisiblePageRange) return effectiveTotal;

            int end = CalculatedStartPage + VisiblePageRange - 1;
            return Math.Min(effectiveTotal, end);
        }
    }

    /// <summary>
    /// Solicita el cambio a la página indicada si es válida y diferente a la actual.
    /// </summary>
    protected async Task SelectPageAsync(int pageNumber)
    {
        if (pageNumber < 1 || pageNumber > Math.Max(1, TotalPages) || pageNumber == CurrentPage)
        {
            return;
        }

        await OnPageChanged.InvokeAsync(pageNumber);
    }
}
