namespace NorthWind.Sales.Frontend.Views.ViewModels;

public class CreateOrderViewModelValidator : AbstractViewModelValidator<CreateOrderDto, CreateOrderViewModel>
{
    public CreateOrderViewModelValidator(
        IValidationService<CreateOrderViewModel> validationService,
        IModelValidator<CreateOrderDetailViewModel> orderDetailValidator)
        : base(validationService)
    {
        RuleFor(o => o.CustomerId)
            .NotEmpty(CreateOrderMessages.CustomerIdRequired)
            .Length(5, CreateOrderMessages.CustomerIdLength);

        RuleFor(o => o.ShipAddress)
            .NotEmpty(CreateOrderMessages.ShipAddressRequired);

        RuleFor(o => o.ShipCity)
            .NotEmpty(CreateOrderMessages.ShipCityRequired);

        RuleFor(o => o.ShipCountry)
            .NotEmpty(CreateOrderMessages.ShipCountryRequired);

        RuleFor(o => o.ShipPostalCode)
            .NotEmpty(CreateOrderMessages.ShipPostalCodeRequired);

        RuleForEach(o => o.OrderDetails)
            .NotEmpty(CreateOrderMessages.OrderDetailsRequired)
            .SetValidator(orderDetailValidator);
    }
}
