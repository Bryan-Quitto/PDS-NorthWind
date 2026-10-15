using System.Text.RegularExpressions;

namespace NorthWind.Sales.Validators.Entities.Common;

public static partial class LocationTextValidator
{
    // Permite letras (incluye acentos y caracteres internacionales Unicode \p{L}),
    // espacios, puntos, guiones y apóstrofes. Rechaza dígitos numéricos y caracteres especiales no permitidos.
    [GeneratedRegex(@"^[\p{L}\s\.\'\-]+$")]
    private static partial Regex LocationRegex();

    public static bool IsValid(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        return LocationRegex().IsMatch(text.Trim());
    }
}
