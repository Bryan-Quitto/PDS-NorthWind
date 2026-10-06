namespace NorthWind.Sales.Backend.Presenters;

public class CreateOrderPresenter : ICreateOrderOutputPort
{
    public int OrderId { get; private set; }

    public Task HandleAsync(int orderId, CancellationToken cancellationToken = default)
    {
        OrderId = orderId;
        return Task.CompletedTask;
    }
}
