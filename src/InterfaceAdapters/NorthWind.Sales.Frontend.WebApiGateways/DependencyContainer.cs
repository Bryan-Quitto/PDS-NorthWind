namespace NorthWind.Sales.Frontend.WebApiGateways;

public static class DependencyContainer
{
    public static IServiceCollection AddNorthWindSalesGateways(
        this IServiceCollection services,
        Action<HttpClient> configureClient,
        Action<IHttpClientBuilder>? configureBuilder = null)
    {
        var createOrderBuilder = services.AddHttpClient<ICreateOrderGateway, CreateOrderGateway>(configureClient);
        var searchBuilder = services.AddHttpClient<ISearchGateway, SearchGateway>(configureClient);

        configureBuilder?.Invoke(createOrderBuilder);
        configureBuilder?.Invoke(searchBuilder);

        return services;
    }
}
