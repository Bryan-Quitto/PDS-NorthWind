using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Validation.Entities.Services;

public class ModelValidatorHub<T>(IEnumerable<IModelValidator<T>> validators) : IModelValidatorHub<T>
{
    public async Task<IReadOnlyList<ValidationError>> ValidateAsync(T model, CancellationToken cancellationToken = default)
    {
        var errors = new List<ValidationError>();
        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(model, cancellationToken);
            if (result.Count > 0)
            {
                errors.AddRange(result);
            }
        }
        return errors;
    }
}
