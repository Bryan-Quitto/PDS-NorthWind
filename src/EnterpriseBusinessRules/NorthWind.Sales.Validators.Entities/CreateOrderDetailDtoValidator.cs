using NorthWind.Sales.Validators.Entities.Resources;

namespace NorthWind.Sales.Validators.Entities;

public class CreateOrderDetailDtoValidator : AbstractModelValidator<CreateOrderDetailDto>
{
    public CreateOrderDetailDtoValidator(IValidationService<CreateOrderDetailDto> validationService)
        : base(validationService)
    {
        RuleFor(d => d.ProductId)
            .GreaterThan(0, CreateOrderMessages.ProductIdGreaterThanZero);

        RuleFor(d => d.UnitPrice)
            .GreaterThan(0, CreateOrderMessages.UnitPriceGreaterThanZero);

        RuleFor(d => d.Quantity)
            .GreaterThan((short)0, CreateOrderMessages.QuantityGreaterThanZero)
            .LessThanOrEqualTo(short.MaxValue, CreateOrderMessages.QuantityExceedsMax);
    }
}
