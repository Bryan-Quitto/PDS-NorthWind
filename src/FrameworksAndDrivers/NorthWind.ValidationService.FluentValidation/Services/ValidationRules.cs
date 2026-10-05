using FluentValidation;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.ValidationService.FluentValidation.Services;

public class ValidationRules<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder)
    : IValidationRules<T, TProperty>
{
    private IRuleBuilder<T, TProperty> currentRuleBuilder = ruleBuilder;

    public IValidationRules<T, TProperty> NotEmpty(string errorMessage)
    {
        currentRuleBuilder = currentRuleBuilder.NotEmpty().WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> NotNull(string errorMessage)
    {
        currentRuleBuilder = currentRuleBuilder.NotNull().WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> Length(int exactLength, string errorMessage)
    {
        if (currentRuleBuilder is IRuleBuilder<T, string> stringBuilder)
        {
            currentRuleBuilder = (IRuleBuilder<T, TProperty>)stringBuilder.Length(exactLength).WithMessage(errorMessage);
        }
        return this;
    }

    public IValidationRules<T, TProperty> Length(int minLength, int maxLength, string errorMessage)
    {
        if (currentRuleBuilder is IRuleBuilder<T, string> stringBuilder)
        {
            currentRuleBuilder = (IRuleBuilder<T, TProperty>)stringBuilder.Length(minLength, maxLength).WithMessage(errorMessage);
        }
        return this;
    }

    public IValidationRules<T, TProperty> GreaterThan(TProperty valueToCompare, string errorMessage)
    {
        currentRuleBuilder = currentRuleBuilder.Must(val =>
        {
            if (val is IComparable comparable && valueToCompare is IComparable)
            {
                return comparable.CompareTo(valueToCompare) > 0;
            }
            return Comparer<TProperty>.Default.Compare(val, valueToCompare) > 0;
        }).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> GreaterThanOrEqualTo(TProperty valueToCompare, string errorMessage)
    {
        currentRuleBuilder = currentRuleBuilder.Must(val =>
        {
            if (val is IComparable comparable && valueToCompare is IComparable)
            {
                return comparable.CompareTo(valueToCompare) >= 0;
            }
            return Comparer<TProperty>.Default.Compare(val, valueToCompare) >= 0;
        }).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> LessThan(TProperty valueToCompare, string errorMessage)
    {
        currentRuleBuilder = currentRuleBuilder.Must(val =>
        {
            if (val is IComparable comparable && valueToCompare is IComparable)
            {
                return comparable.CompareTo(valueToCompare) < 0;
            }
            return Comparer<TProperty>.Default.Compare(val, valueToCompare) < 0;
        }).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> LessThanOrEqualTo(TProperty valueToCompare, string errorMessage)
    {
        currentRuleBuilder = currentRuleBuilder.Must(val =>
        {
            if (val is IComparable comparable && valueToCompare is IComparable)
            {
                return comparable.CompareTo(valueToCompare) <= 0;
            }
            return Comparer<TProperty>.Default.Compare(val, valueToCompare) <= 0;
        }).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> Must(Func<TProperty, bool> predicate, string errorMessage)
    {
        currentRuleBuilder = currentRuleBuilder.Must(predicate).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> MustAsync(Func<TProperty, CancellationToken, Task<bool>> predicate, string errorMessage)
    {
        currentRuleBuilder = currentRuleBuilder.MustAsync(predicate).WithMessage(errorMessage);
        return this;
    }
}
