using System.Text.RegularExpressions;

namespace NorthWind.Membership.Entities.Validators;

public static partial class UserNameRulesValidator
{
    [GeneratedRegex(@"^[\p{L}]+(?:['\s\-][\p{L}]+)*$")]
    private static partial Regex ValidNameRegex();

    public static bool IsValidFormat(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (name.Length > 100)
        {
            return false;
        }

        // Rechaza directamente si tiene espacios al inicio o al final
        if (name != name.Trim())
        {
            return false;
        }

        return ValidNameRegex().IsMatch(name);
    }
}
