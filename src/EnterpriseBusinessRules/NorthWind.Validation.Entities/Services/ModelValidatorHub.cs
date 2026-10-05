using NorthWind.Validation.Entities.Enums;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Validation.Entities.Services;

public class ModelValidatorHub<T>(IEnumerable<IModelValidator<T>> validators) : IModelValidatorHub<T>
{
    public async Task<IReadOnlyList<ValidationError>> ValidateAsync(T model, CancellationToken cancellationToken = default)
    {
        var errors = new List<ValidationError>();

        // 1. Fase AlwaysValidate (p. ej. validaciones en memoria / sintácticas de formato)
        foreach (var validator in validators.Where(v => v.Constraint == ValidationConstraint.AlwaysValidate))
        {
            var result = await validator.ValidateAsync(model, cancellationToken);
            if (result.Count > 0)
            {
                errors.AddRange(result);
            }
        }

        // 2. Short-circuit: Si hay errores previos, omitir validadores costosos que dependen de la base de datos
        if (errors.Count > 0)
        {
            return errors;
        }

        // 3. Fase ValidateIfThereAreNoPreviousErrors (p. ej. consultas a SQL Server / existencia de cliente / inventario)
        foreach (var validator in validators.Where(v => v.Constraint == ValidationConstraint.ValidateIfThereAreNoPreviousErrors))
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
