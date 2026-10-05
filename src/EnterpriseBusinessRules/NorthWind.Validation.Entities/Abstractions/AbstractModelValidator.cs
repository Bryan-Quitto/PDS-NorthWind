using NorthWind.Validation.Entities.Enums;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Validation.Entities.Abstractions;

public abstract class AbstractModelValidator<T>(
    IValidationService<T> validationService,
    ValidationConstraint constraint = ValidationConstraint.AlwaysValidate) : IModelValidator<T>
{
    public ValidationConstraint Constraint { get; } = constraint;

    protected IValidationRules<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression) =>
        validationService.RuleFor(expression);

    protected ICollectionValidationRules<T, TElement> RuleForEach<TElement>(Expression<Func<T, IEnumerable<TElement>>> expression) =>
        validationService.RuleForEach(expression);

    public virtual Task<IReadOnlyList<ValidationError>> ValidateAsync(T model, CancellationToken cancellationToken = default) =>
        validationService.ValidateAsync(model, cancellationToken);
}
