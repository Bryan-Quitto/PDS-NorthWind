using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NorthWind.Membership.Frontend.RazorViews.Services;
using NorthWind.Membership.Frontend.RazorViews.ViewModels;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Membership.Frontend.RazorViews.DependencyInjection;

public static class MembershipRazorViewsServiceCollectionExtensions
{
    public static IServiceCollection AddMembershipFrontendServices(
        this IServiceCollection services,
        string baseAddress)
    {
        services.AddScoped<ITokenStorage, TokenLocalStorage>();
        services.AddScoped<JWTAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JWTAuthenticationStateProvider>());
        services.AddAuthorizationCore();

        services.AddTransient<MembershipBearerTokenHandler>();

        services.AddHttpClient<MembershipGateway>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        });

        // Validadores para ModelValidator<T>
        services.AddTransient<IModelValidator<UserRegistrationViewModel>, UserRegistrationViewModelValidator>();
        services.AddTransient<IModelValidator<UserLoginViewModel>, UserLoginViewModelValidator>();

        services.AddTransient<UserRegistrationViewModel>();
        services.AddTransient<UserLoginViewModel>();

        return services;
    }
}
