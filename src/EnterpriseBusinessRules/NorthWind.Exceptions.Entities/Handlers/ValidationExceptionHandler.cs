using NorthWind.Exceptions.Entities.Exceptions;
using NorthWind.Exceptions.Entities.Extensions;

namespace NorthWind.Exceptions.Entities.Handlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Title = "Error de validación",
            Detail = validationException.Message,
            Instance = httpContext.Request.Path
        };

        var errorsGrouped = new Dictionary<string, string[]>();
        foreach (var error in validationException.Errors)
        {
            if (errorsGrouped.TryGetValue(error.PropertyName, out var existing))
            {
                errorsGrouped[error.PropertyName] = [.. existing, error.ErrorMessage];
            }
            else
            {
                errorsGrouped[error.PropertyName] = [error.ErrorMessage];
            }
        }

        problemDetails.Extensions["errors"] = errorsGrouped;

        await httpContext.WriteProblemDetailsAsync(problemDetails, cancellationToken);
        return true;
    }
}
