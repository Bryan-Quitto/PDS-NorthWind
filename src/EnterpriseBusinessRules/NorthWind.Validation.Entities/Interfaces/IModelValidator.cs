using NorthWind.Validation.Entities.Enums;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Validation.Entities.Interfaces;

public interface IModelValidator<T>
{
    ValidationConstraint Constraint => ValidationConstraint.AlwaysValidate;
    Task<IReadOnlyList<ValidationError>> ValidateAsync(T model, CancellationToken cancellationToken = default);
}
