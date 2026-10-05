#nullable enable
namespace NorthWind.Sales.Validators.Entities.Resources;

public class CreateOrderMessages
{
    private static global::System.Resources.ResourceManager? resourceMan;
    private static global::System.Globalization.CultureInfo? resourceCulture;

    internal CreateOrderMessages() { }

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    public static global::System.Resources.ResourceManager ResourceManager
    {
        get
        {
            if (object.ReferenceEquals(resourceMan, null))
            {
                global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NorthWind.Sales.Validators.Entities.Resources.CreateOrderMessages", typeof(CreateOrderMessages).Assembly);
                resourceMan = temp;
            }
            return resourceMan;
        }
    }

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    public static global::System.Globalization.CultureInfo? Culture
    {
        get => resourceCulture;
        set => resourceCulture = value;
    }

    public static string CustomerIdRequired => ResourceManager.GetString("CustomerIdRequired", resourceCulture) ?? "El identificador del cliente es requerido.";
    public static string CustomerIdLength => ResourceManager.GetString("CustomerIdLength", resourceCulture) ?? "El identificador del cliente debe tener exactamente 5 caracteres.";
    public static string ShipAddressRequired => ResourceManager.GetString("ShipAddressRequired", resourceCulture) ?? "La dirección de envío es requerida.";
    public static string ShipCityRequired => ResourceManager.GetString("ShipCityRequired", resourceCulture) ?? "La ciudad de envío es requerida.";
    public static string ShipCountryRequired => ResourceManager.GetString("ShipCountryRequired", resourceCulture) ?? "El país de envío es requerido.";
    public static string ShipPostalCodeRequired => ResourceManager.GetString("ShipPostalCodeRequired", resourceCulture) ?? "El código postal es requerido.";
    public static string OrderDetailsRequired => ResourceManager.GetString("OrderDetailsRequired", resourceCulture) ?? "La orden debe contener al menos un detalle de producto.";
    public static string ProductIdGreaterThanZero => ResourceManager.GetString("ProductIdGreaterThanZero", resourceCulture) ?? "El identificador del producto debe ser mayor a cero.";
    public static string UnitPriceGreaterThanZero => ResourceManager.GetString("UnitPriceGreaterThanZero", resourceCulture) ?? "El precio unitario debe ser mayor a cero.";
    public static string QuantityGreaterThanZero => ResourceManager.GetString("QuantityGreaterThanZero", resourceCulture) ?? "La cantidad debe ser mayor a cero.";
}
