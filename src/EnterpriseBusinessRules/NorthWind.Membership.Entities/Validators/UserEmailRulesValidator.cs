using System.Text.RegularExpressions;

namespace NorthWind.Membership.Entities.Validators;

public static partial class UserEmailRulesValidator
{
    // Solo caracteres alfanuméricos, punto, guion y guion bajo antes de la @, y dominio válido
    [GeneratedRegex(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex ValidEmailRegex();

    public static bool HasNoConsecutiveDots(string? email)
    {
        if (string.IsNullOrEmpty(email)) return true;
        return !email.Contains("..");
    }

    public static bool IsValidFormat(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        if (email.Length > 256)
        {
            return false;
        }

        if (!HasNoConsecutiveDots(email))
        {
            return false;
        }

        return ValidEmailRegex().IsMatch(email);
    }
}
