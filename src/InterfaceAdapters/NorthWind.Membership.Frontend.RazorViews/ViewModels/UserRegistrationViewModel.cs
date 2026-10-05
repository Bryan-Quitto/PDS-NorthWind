using NorthWind.Membership.Entities.Dtos;
using NorthWind.Membership.Frontend.RazorViews.Services;
using NorthWind.Result.Entities;

namespace NorthWind.Membership.Frontend.RazorViews.ViewModels;

public class UserRegistrationViewModel(MembershipGateway gateway)
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
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

        var dto = new UserRegistrationDto
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
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
