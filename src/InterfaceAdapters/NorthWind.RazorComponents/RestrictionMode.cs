namespace NorthWind.RazorComponents;

/// <summary>
/// Modos de restricción de caracteres permitidos para el componente RestrictedInputText.
/// </summary>
public enum RestrictionMode
{
    /// <summary>
    /// Sin restricción de caracteres.
    /// </summary>
    None = 0,

    /// <summary>
    /// Permite únicamente letras (incluyendo vocales con tildes y diéresis comunes).
    /// </summary>
    OnlyLetters = 1,

    /// <summary>
    /// Permite únicamente letras y espacios en blanco.
    /// </summary>
    LettersAndSpaces = 2,

    /// <summary>
    /// Permite únicamente dígitos numéricos (0-9).
    /// </summary>
    OnlyDigits = 3,

    /// <summary>
    /// Permite únicamente caracteres alfanuméricos (letras y dígitos).
    /// </summary>
    Alphanumeric = 4,

    /// <summary>
    /// Permite formatos de códigos postales comunes (letras, dígitos, guiones y espacios).
    /// </summary>
    PostalCode = 5,

    /// <summary>
    /// Filtra caracteres según una expresión regular personalizada especificada en CustomPattern.
    /// </summary>
    CustomRegex = 6
}
