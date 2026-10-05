namespace NorthWind.Validation.Entities.Interfaces;

public interface IValidationRules<T, TProperty>
{
    IValidationRules<T, TProperty> NotEmpty(string errorMessage);
    IValidationRules<T, TProperty> NotNull(string errorMessage);
    IValidationRules<T, TProperty> Length(int exactLength, string errorMessage);
    IValidationRules<T, TProperty> Length(int minLength, int maxLength, string errorMessage);
    IValidationRules<T, TProperty> GreaterThan(TProperty valueToCompare, string errorMessage);
    IValidationRules<T, TProperty> GreaterThanOrEqualTo(TProperty valueToCompare, string errorMessage);
    IValidationRules<T, TProperty> LessThan(TProperty valueToCompare, string errorMessage);
    IValidationRules<T, TProperty> LessThanOrEqualTo(TProperty valueToCompare, string errorMessage);
    IValidationRules<T, TProperty> Must(Func<TProperty, bool> predicate, string errorMessage);
    IValidationRules<T, TProperty> MustAsync(Func<TProperty, CancellationToken, Task<bool>> predicate, string errorMessage);
    IValidationRules<T, TProperty> Equal(Func<T, TProperty> expression, string errorMessage);
}
