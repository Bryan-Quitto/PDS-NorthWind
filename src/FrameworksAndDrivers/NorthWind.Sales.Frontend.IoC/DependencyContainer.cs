namespace NorthWind.Sales.Frontend.IoC;

public static class DependencyContainer
{
    public static IServiceCollection AddNorthWindSalesFrontendServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var webApiAddress = configuration["WebApiAddress"] ?? "https://localhost:7001/";

        services.AddTransient<ExceptionDelegatingHandler>();

        services.AddNorthWindSalesGateway(client =>
        {
            client.BaseAddress = new Uri(webApiAddress);
        })
        .AddHttpMessageHandler<ExceptionDelegatingHandler>();

        services.AddTransient(typeof(IValidationService<>), typeof(ValidationService<>));
        services.AddTransient<IModelValidator<CreateOrderDetailViewModel>, CreateOrderDetailViewModelValidator>();
        services.AddTransient<IModelValidator<CreateOrderViewModel>, CreateOrderViewModelValidator>();
        services.AddScoped(typeof(IModelValidatorHub<>), typeof(ModelValidatorHub<>));

        services.AddTransient<CreateOrderViewModel>();

        return services;
    }
}
