namespace NorthWind.Validation.Entities.Interfaces;

public interface IValidationService<T> : IModelValidator<T>
{
    IValidationRules<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression);
    ICollectionValidationRules<T, TElement> RuleForEach<TElement>(Expression<Func<T, IEnumerable<TElement>>> expression);
}
