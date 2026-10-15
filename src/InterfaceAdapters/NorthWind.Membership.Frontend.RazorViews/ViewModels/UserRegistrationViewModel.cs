using NorthWind.Membership.Entities.Dtos;
using NorthWind.Membership.Frontend.RazorViews.Services;
using NorthWind.Result.Entities;

namespace NorthWind.Membership.Frontend.RazorViews.ViewModels;

public class UserRegistrationViewModel(MembershipGateway gateway)
{
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty;

    public string FirstName
    {
        get => _firstName;
        set => _firstName = System.Text.RegularExpressions.Regex.Replace(value ?? string.Empty, @"[^\p{L}\s\-']", "");
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = System.Text.RegularExpressions.Regex.Replace(value ?? string.Empty, @"[^\p{L}\s\-']", "");
    }

    public string Email
    {
        get => _email;
        set => _email = System.Text.RegularExpressions.Regex.Replace(value ?? string.Empty, @"[^a-zA-Z0-9._@-]", "");
    }

    public string Password { get; set; } = string.Empty;
    public string PasswordConfirmation { get; set; } = string.Empty;

    public List<string> Errors { get; private set; } = [];
    public bool IsSuccess { get; private set; }
    public bool IsLoading { get; private set; }

    public async Task<bool> RegisterAsync()
    {
        IsLoading = true;
        Errors.Clear();
        IsSuccess = false;

        // Limpieza real (Trim) al mapear el DTO para persistencia limpia
        var dto = new UserRegistrationDto
        {
            FirstName = FirstName.Trim(),
            LastName = LastName.Trim(),
            Email = Email.Trim(),
            Password = Password
        };

        try
        {
            var result = await gateway.RegisterAsync(dto);
            result.HandleResult(
                () =>
                {
                    IsSuccess = true;
                    FirstName = string.Empty;
                    LastName = string.Empty;
                    Email = string.Empty;
                    Password = string.Empty;
                    PasswordConfirmation = string.Empty;
                },
                errors =>
                {
                    Errors = [.. errors];
                }
            );
        }
        catch (HttpRequestException)
        {
            Errors = ["No se pudo conectar con el servidor backend. Verifique que la Web API esté ejecutándose."];
        }
        catch (Exception ex)
        {
            Errors = [$"Error inesperado: {ex.Message}"];
        }
        finally
        {
            IsLoading = false;
        }
        return IsSuccess;
    }
}
