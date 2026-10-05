using Microsoft.JSInterop;

namespace NorthWind.Membership.Frontend.RazorViews.Services;

public class TokenLocalStorage(IJSRuntime jsRuntime) : ITokenStorage
{
    private const string TokenKey = "northwind_token";

    public async ValueTask<string?> GetTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
    }

    public async ValueTask SetTokenAsync(string token)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
    }

    public async ValueTask RemoveTokenAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    }
}
