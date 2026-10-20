namespace NorthWind.RazorComponents;

/// <summary>
/// Componente de indicador de carga reutilizable basado en Bootstrap 5.
/// </summary>
public partial class LoadingSpinner : ComponentBase
{
    /// <summary>
    /// Texto explicativo que acompaña al indicador de carga (por defecto: "Cargando...").
    /// </summary>
    [Parameter]
    public string? Text { get; set; } = "Cargando...";

    /// <summary>
    /// Tamaño del indicador de carga (Normal o Small).
    /// </summary>
    [Parameter]
    public SpinnerSize Size { get; set; } = SpinnerSize.Normal;

    /// <summary>
    /// Color temático de Bootstrap (primary, secondary, success, danger, warning, info, light, dark).
    /// Por defecto es "primary".
    /// </summary>
    [Parameter]
    public string Color { get; set; } = "primary";

    /// <summary>
    /// Determina si el spinner se centra horizontal y verticalmente con espaciado.
    /// Por defecto es true.
    /// </summary>
    [Parameter]
    public bool IsCentered { get; set; } = true;

    /// <summary>
    /// Determina si se renderiza en línea (inline-flex) en lugar de un bloque centrado.
    /// </summary>
    [Parameter]
    public bool Inline { get; set; } = false;

    /// <summary>
    /// Clases CSS adicionales para el contenedor exterior del spinner.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Clases CSS aplicadas al elemento del spinner.
    /// </summary>
    protected string SpinnerClass => Size switch
    {
        SpinnerSize.Small => "spinner-border spinner-border-sm",
        _ => "spinner-border"
    };

    /// <summary>
    /// Clase de color Bootstrap aplicada al spinner.
    /// </summary>
    protected string ColorClass => $"text-{Color}";

    /// <summary>
    /// Clases CSS del contenedor principal según la alineación y estilo configurados.
    /// </summary>
    protected string ContainerClass
    {
        get
        {
            var baseStyle = Inline
                ? "d-inline-flex align-items-center gap-2"
                : IsCentered
                    ? "d-flex flex-column align-items-center justify-content-center py-4"
                    : "d-flex align-items-center gap-2";

            return string.IsNullOrWhiteSpace(Class) ? baseStyle : $"{baseStyle} {Class}";
        }
    }
}
