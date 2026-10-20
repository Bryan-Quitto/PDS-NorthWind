namespace NorthWind.RazorComponents;

/// <summary>
/// Representa un criterio de búsqueda disponible para el componente CriteriaSearchBar.
/// </summary>
public sealed class SearchCriterion
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="SearchCriterion"/>.
    /// </summary>
    /// <param name="key">Identificador único o clave técnica del criterio.</param>
    /// <param name="label">Etiqueta visible para el usuario.</param>
    /// <param name="icon">Clase de icono opcional (ej. "bi bi-person").</param>
    public SearchCriterion(string key, string label, string? icon = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(label);

        Key = key;
        Label = label;
        Icon = icon;
    }

    /// <summary>
    /// Clave técnica o identificador único del criterio.
    /// </summary>
    public string Key { get; init; }

    /// <summary>
    /// Etiqueta descriptiva que se muestra en el menú desplegable y en el botón selector.
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// Clase de icono CSS (por ejemplo de Bootstrap Icons) asociada al criterio.
    /// </summary>
    public string? Icon { get; init; }
}
