namespace NorthWind.Sales.Frontend.Views.Components;

public partial class ShippingAddressSection : ComponentBase
{
    [Parameter]
    public CreateOrderViewModel Model { get; set; } = default!;
}
