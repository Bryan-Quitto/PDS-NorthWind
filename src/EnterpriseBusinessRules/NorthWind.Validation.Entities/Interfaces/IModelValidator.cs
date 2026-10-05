using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Validation.Entities.Interfaces;

public interface IModelValidator<T>
{
    Task<IReadOnlyList<ValidationError>> ValidateAsync(T model, CancellationToken cancellationToken = default);
}
