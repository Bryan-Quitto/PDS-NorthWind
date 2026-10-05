using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Validation.Entities.Interfaces;

public interface IModelValidatorHub<T>
{
    Task<IReadOnlyList<ValidationError>> ValidateAsync(T model, CancellationToken cancellationToken = default);
}
