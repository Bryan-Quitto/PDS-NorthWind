namespace NorthWind.Membership.Entities.Resources;

public static class MembershipMessages
{
    public const string FirstNameRequired = "El nombre es requerido.";
    public const string LastNameRequired = "El apellido es requerido.";
    public const string EmailRequired = "El correo electrónico es requerido.";
    public const string EmailInvalid = "El formato del correo electrónico no es válido.";
    public const string PasswordRequired = "La contraseña es requerida.";
    public const string PasswordTooShort = "La contraseña debe tener al menos 6 caracteres.";
    public const string PasswordRequiresLower = "Se requiere al menos un caracter minúscula en la contraseña.";
    public const string PasswordRequiresUpper = "Se requiere al menos un caracter mayúscula en la contraseña.";
    public const string PasswordRequiresDigit = "Se requiere al menos un dígito en la contraseña.";
    public const string PasswordRequiresNonAlphanumeric = "Se requiere al menos un caracter no alfanumérico en la contraseña.";
    public const string UserNameRequired = "El nombre de usuario es requerido.";
    public const string InvalidCredentials = "Las credenciales proporcionadas son inválidas.";
    public const string UserRegistrationFailed = "Error en el registro del usuario.";
}
