namespace NorthWind.Sales.Frontend.Views.ViewModels;

public class CreateOrderDetailViewModelValidator : AbstractViewModelValidator<CreateOrderDetailDto, CreateOrderDetailViewModel>
{
    public CreateOrderDetailViewModelValidator(IValidationService<CreateOrderDetailViewModel> validationService)
        : base(validationService)
    {
        RuleFor(d => d.ProductId)
            .GreaterThan(0, CreateOrderMessages.ProductIdGreaterThanZero);

        RuleFor(d => d.UnitPrice)
            .GreaterThan(0, CreateOrderMessages.UnitPriceGreaterThanZero);

        RuleFor(d => d.RawQuantity)
            .NotEmpty(CreateOrderMessages.QuantityGreaterThanZero)
            .Must(raw => short.TryParse(raw, out _), CreateOrderMessages.QuantityMustBeInteger);

        RuleFor(d => d.Quantity)
            .GreaterThan((short)0, CreateOrderMessages.QuantityGreaterThanZero);
    }
}
