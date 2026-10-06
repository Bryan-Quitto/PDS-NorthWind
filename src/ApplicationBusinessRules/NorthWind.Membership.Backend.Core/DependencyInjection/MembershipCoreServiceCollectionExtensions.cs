using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthWind.Membership.Backend.Core.Controllers;
using NorthWind.Membership.Backend.Core.Interfaces.Ports;
using NorthWind.Membership.Backend.Core.Options;
using NorthWind.Membership.Backend.Core.Presenters;
using NorthWind.Membership.Backend.Core.Services;
using NorthWind.Membership.Backend.Core.UseCases;
using NorthWind.Membership.Entities.Validators;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Membership.Backend.Core.DependencyInjection;

public static class MembershipCoreServiceCollectionExtensions
{
    public static IServiceCollection AddMembershipCoreServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionKey));
        services.AddSingleton<JwtService>();

        // Validadores
        services.AddTransient<IModelValidator<UserRegistrationDto>, UserRegistrationDtoValidator>();
        services.AddTransient<IModelValidator<UserCredentialsDto>, UserCredentialsDtoValidator>();

        // Registro
        services.AddScoped<UserRegistrationPresenter>();
        services.AddScoped<IUserRegistrationOutputPort>(sp => sp.GetRequiredService<UserRegistrationPresenter>());
        services.AddScoped<IUserRegistrationInputPort, UserRegistrationInteractor>();

        // Login
        services.AddScoped<UserLoginPresenter>();
        services.AddScoped<IUserLoginOutputPort>(sp => sp.GetRequiredService<UserLoginPresenter>());
        services.AddScoped<IUserLoginInputPort, UserLoginInteractor>();

        return services;
    }

    public static WebApplication MapMembershipEndpoints(this WebApplication app)
    {
        app.UseUserRegistrationController();
        app.UseUserLoginController();
        return app;
    }
}
