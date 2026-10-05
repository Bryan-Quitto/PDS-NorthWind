using NorthWind.Membership.Entities.Dtos;
using NorthWind.Membership.Frontend.RazorViews.Services;
using NorthWind.Result.Entities;

namespace NorthWind.Membership.Frontend.RazorViews.ViewModels;

public class UserLoginViewModel(
    MembershipGateway gateway,
    ITokenStorage tokenStorage,
    JWTAuthenticationStateProvider authStateProvider)
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public List<string> Errors { get; private set; } = [];
    public bool IsLoading { get; private set; }

    public async Task<bool> LoginAsync()
    {
        IsLoading = true;
        Errors.Clear();

        var credentials = new UserCredentialsDto
        {
            Email = Email,
            Password = Password
        };

        var success = false;
        try
        {
            var result = await gateway.LoginAsync(credentials);
            if (!result.HasError && result.SuccessValue != null)
            {
                var token = result.SuccessValue.AccessToken;
                await tokenStorage.SetTokenAsync(token);
                authStateProvider.NotifyUserAuthentication(token);
                success = true;
            }
            else if (result.ErrorValue != null)
            {
                Errors = [.. result.ErrorValue];
            }
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

        return success;
    }

    public async Task LogoutAsync()
    {
        await tokenStorage.RemoveTokenAsync();
        authStateProvider.NotifyUserLogout();
    }
}
