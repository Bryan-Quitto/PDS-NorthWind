namespace NorthWind.Sales.Frontend.IoC;

public static class DependencyContainer
{
    public static IServiceCollection AddNorthWindSalesFrontendServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var webApiAddress = configuration["WebApiAddress"] ?? "https://localhost:7001/";

        // Servicios Frontend de Membresía
        NorthWind.Membership.Frontend.RazorViews.DependencyInjection.MembershipRazorViewsServiceCollectionExtensions.AddMembershipFrontendServices(services, webApiAddress);

        services.AddTransient<ExceptionDelegatingHandler>();

        services.AddNorthWindSalesGateway(client =>
        {
            client.BaseAddress = new Uri(webApiAddress);
        })
        .AddHttpMessageHandler<NorthWind.Membership.Frontend.RazorViews.Services.MembershipBearerTokenHandler>()
        .AddHttpMessageHandler<ExceptionDelegatingHandler>();

        services.AddTransient(typeof(IValidationService<>), typeof(ValidationService<>));
        services.AddTransient<IModelValidator<CreateOrderDetailViewModel>, CreateOrderDetailViewModelValidator>();
        services.AddTransient<IModelValidator<CreateOrderViewModel>, CreateOrderViewModelValidator>();
        services.AddScoped(typeof(IModelValidatorHub<>), typeof(ModelValidatorHub<>));

        services.AddTransient<CreateOrderViewModel>();

        return services;
    }
}
