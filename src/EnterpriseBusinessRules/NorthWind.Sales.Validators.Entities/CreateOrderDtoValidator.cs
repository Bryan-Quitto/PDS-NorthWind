using NorthWind.Sales.Validators.Entities.Common;
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
            .NotEmpty(CreateOrderMessages.ShipAddressRequired)
            .Length(1, 60, CreateOrderMessages.ShipAddressMaxLength);

        RuleFor(o => o.ShipCity)
            .NotEmpty(CreateOrderMessages.ShipCityRequired)
            .Length(1, 15, CreateOrderMessages.ShipCityMaxLength)
            .Must(LocationTextValidator.IsValid, CreateOrderMessages.ShipCityInvalid);

        RuleFor(o => o.ShipCountry)
            .NotEmpty(CreateOrderMessages.ShipCountryRequired)
            .Length(1, 15, CreateOrderMessages.ShipCountryMaxLength)
            .Must(LocationTextValidator.IsValid, CreateOrderMessages.ShipCountryInvalid);

        RuleFor(o => o.ShipPostalCode)
            .NotEmpty(CreateOrderMessages.ShipPostalCodeRequired)
            .Length(1, 10, CreateOrderMessages.ShipPostalCodeMaxLength);

        RuleForEach(o => o.OrderDetails)
            .NotEmpty(CreateOrderMessages.OrderDetailsRequired)
            .SetValidator(orderDetailValidator);
    }
}
