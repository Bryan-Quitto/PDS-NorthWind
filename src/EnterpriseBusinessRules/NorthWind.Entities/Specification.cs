using System.Linq.Expressions;

namespace NorthWind.Entities;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ConditionExpression { get; }

    public bool IsSatisfiedBy(T entityInstance)
    {
        Func<T, bool> predicate = ConditionExpression.Compile();
        return predicate(entityInstance);
    }
}
