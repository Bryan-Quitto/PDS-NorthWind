using NorthWind.Sales.Validators.Entities.Resources;

namespace NorthWind.Sales.Validators.Entities;

public class CreateOrderDtoValidator : AbstractModelValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator(
        IValidationService<CreateOrderDto> validationService,
        IModelValidator<CreateOrderDetailDto> orderDetailValidator)
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
