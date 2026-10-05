using System.Linq.Expressions;
using FluentValidation;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.ValidationService.FluentValidation.Services;

public class ValidationService<T> : InlineValidator<T>, IValidationService<T>
{
    public new IValidationRules<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var ruleBuilder = base.RuleFor(expression);
        return new ValidationRules<T, TProperty>(ruleBuilder);
    }

    public new ICollectionValidationRules<T, TElement> RuleForEach<TElement>(Expression<Func<T, IEnumerable<TElement>>> expression)
    {
        var ruleBuilder = base.RuleForEach(expression);
        return new CollectionValidationRules<T, TElement>(ruleBuilder);
    }

    public new async Task<IReadOnlyList<ValidationError>> ValidateAsync(T model, CancellationToken cancellationToken = default)
    {
        var result = await base.ValidateAsync(model, cancellationToken);
        if (result.IsValid)
        {
            return [];
        }

        return result.Errors
            .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))
            .ToList();
    }
}
