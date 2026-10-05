namespace NorthWind.Membership.Frontend.RazorViews.Services;

public interface ITokenStorage
{
    ValueTask<string?> GetTokenAsync();
    ValueTask SetTokenAsync(string token);
    ValueTask RemoveTokenAsync();
}
