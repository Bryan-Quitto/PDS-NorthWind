namespace NorthWind.Sales.Frontend.WebApiGateways;

public static class DependencyContainer
{
    public static IHttpClientBuilder AddNorthWindSalesGateway(
        this IServiceCollection services,
        Action<HttpClient> configureClient)
    {
        return services.AddHttpClient<ICreateOrderGateway, CreateOrderGateway>(configureClient);
    }
}
