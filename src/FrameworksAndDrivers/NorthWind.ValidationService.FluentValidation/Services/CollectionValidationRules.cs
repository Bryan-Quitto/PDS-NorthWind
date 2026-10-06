namespace NorthWind.ValidationService.FluentValidation.Services;

public class CollectionValidationRules<T, TElement>(IRuleBuilderInitialCollection<T, TElement> ruleBuilder)
    : ICollectionValidationRules<T, TElement>
{
    private IRuleBuilder<T, TElement> currentRuleBuilder = ruleBuilder;

    public ICollectionValidationRules<T, TElement> NotEmpty(string errorMessage)
    {
        currentRuleBuilder = currentRuleBuilder.NotEmpty().WithMessage(errorMessage);
        return this;
    }

    public ICollectionValidationRules<T, TElement> SetValidator(IModelValidator<TElement> validator)
    {
        currentRuleBuilder = currentRuleBuilder.CustomAsync(async (element, context, cancellationToken) =>
        {
            var errors = await validator.ValidateAsync(element, cancellationToken);
            foreach (var error in errors)
            {
                context.AddFailure(error.PropertyName, error.ErrorMessage);
            }
        });
        return this;
    }
}
