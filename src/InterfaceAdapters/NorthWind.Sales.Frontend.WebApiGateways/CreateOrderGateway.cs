namespace NorthWind.Sales.Frontend.WebApiGateways;

public class CreateOrderGateway(HttpClient client) : ICreateOrderGateway
{
    public async Task<int> CreateOrderAsync(CreateOrderDto orderDto, CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsJsonAsync(Endpoints.CreateOrder, orderDto, cancellationToken);
        response.EnsureSuccessStatusCode();

        var orderId = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return orderId;
    }
}
