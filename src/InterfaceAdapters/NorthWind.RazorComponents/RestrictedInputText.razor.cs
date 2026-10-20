using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace NorthWind.RazorComponents;

/// <summary>
/// Componente de entrada de texto con filtrado en tiempo real de caracteres en C# sin scripts inline.
/// Hereda de <see cref="InputBase{TValue}"/> para soporte completo con EditForm y validación.
/// </summary>
public partial class RestrictedInputText : InputBase<string>
{
    /// <summary>
    /// Modo de restricción de caracteres a aplicar (None, OnlyLetters, OnlyDigits, etc.).
    /// </summary>
    [Parameter]
    public RestrictionMode Mode { get; set; } = RestrictionMode.None;

    /// <summary>
    /// Expresión regular personalizada utilizada cuando <see cref="Mode"/> es <see cref="RestrictionMode.CustomRegex"/>.
    /// Los caracteres que coincidan con este patrón serán los únicos admitidos.
    /// </summary>
    [Parameter]
    public string? CustomPattern { get; set; }

    /// <summary>
    /// Longitud máxima permitida de caracteres.
    /// </summary>
    [Parameter]
    public int? MaxLength { get; set; }

    /// <summary>
    /// Texto de marcador de posición (placeholder) dentro del input.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Modo de entrada para teclados virtuales en dispositivos móviles (inputmode).
    /// </summary>
    [Parameter]
    public string? InputMode { get; set; }

    /// <summary>
    /// Calcula las clases CSS del control asegurando la presencia de 'form-control' de Bootstrap
    /// e integrando las clases de estado de validación de Blazor.
    /// </summary>
    protected string FormInputClass
    {
        get
        {
            var baseClasses = CssClass;
            if (string.IsNullOrWhiteSpace(baseClasses))
            {
                return "form-control";
            }

            var parts = baseClasses.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!parts.Any(p => p.StartsWith("form-control", StringComparison.OrdinalIgnoreCase)))
            {
                return $"form-control {baseClasses}";
            }

            return baseClasses;
        }
    }

    /// <summary>
    /// Obtiene el modo de entrada HTML5 óptimo según la restricción configurada.
    /// </summary>
    protected string ResolvedInputMode => !string.IsNullOrEmpty(InputMode)
        ? InputMode
        : Mode switch
        {
            RestrictionMode.OnlyDigits => "numeric",
            RestrictionMode.PostalCode => "text",
            _ => "text"
        };

    /// <summary>
    /// Diccionario de atributos adicionales excluyendo 'class' para evitar duplicidad en el DOM.
    /// </summary>
    protected IReadOnlyDictionary<string, object>? FilteredAttributes
    {
        get
        {
            if (AdditionalAttributes == null || !AdditionalAttributes.ContainsKey("class"))
            {
                return AdditionalAttributes;
            }

            return AdditionalAttributes
                .Where(kvp => !string.Equals(kvp.Key, "class", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(
        string? value,
        out string result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = FilterValue(value ?? string.Empty);
        validationErrorMessage = null;
        return true;
    }

    /// <summary>
    /// Maneja el evento de entrada en tiempo real (input) filtrando inmediatamente los caracteres.
    /// </summary>
    protected async Task HandleInputAsync(ChangeEventArgs e)
    {
        var rawValue = e.Value?.ToString() ?? string.Empty;
        var sanitized = FilterValue(rawValue);

        CurrentValueAsString = sanitized;
        await ValueChanged.InvokeAsync(CurrentValue);
        StateHasChanged();
    }

    /// <summary>
    /// Aplica las reglas de filtrado de caracteres y longitud máxima al texto recibido.
    /// </summary>
    protected string FilterValue(string raw)
    {
        if (string.IsNullOrEmpty(raw))
        {
            return string.Empty;
        }

        string filtered = Mode switch
        {
            RestrictionMode.OnlyLetters => Regex.Replace(raw, @"[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ]", string.Empty),
            RestrictionMode.LettersAndSpaces => Regex.Replace(raw, @"[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]", string.Empty),
            RestrictionMode.OnlyDigits => Regex.Replace(raw, @"[^\d]", string.Empty),
            RestrictionMode.Alphanumeric => Regex.Replace(raw, @"[^a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ]", string.Empty),
            RestrictionMode.PostalCode => Regex.Replace(raw, @"[^a-zA-Z0-9\-\s]", string.Empty),
            RestrictionMode.CustomRegex when !string.IsNullOrEmpty(CustomPattern) => FilterWithCustomPattern(raw, CustomPattern),
            _ => raw
        };

        if (MaxLength.HasValue && MaxLength.Value > 0 && filtered.Length > MaxLength.Value)
        {
            filtered = filtered[..MaxLength.Value];
        }

        return filtered;
    }

    private static string FilterWithCustomPattern(string raw, string pattern)
    {
        try
        {
            var regex = new Regex(pattern);
            var matchingChars = raw.Where(c => regex.IsMatch(c.ToString())).ToArray();
            return new string(matchingChars);
        }
        catch (ArgumentException)
        {
            return raw;
        }
    }
}
