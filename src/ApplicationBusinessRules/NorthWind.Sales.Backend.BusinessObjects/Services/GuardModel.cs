using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Sales.Backend.BusinessObjects.Services;

public static class GuardModel
{
    public static async Task AgainstErrorsAsync<T>(
        IModelValidatorHub<T> validatorHub,
        T model,
        CancellationToken cancellationToken = default)
    {
        var errors = await validatorHub.ValidateAsync(model, cancellationToken);
        if (errors.Count > 0)
        {
                throw new NorthWind.Exceptions.Entities.Exceptions.ValidationException(errors);
        }
    }
}
