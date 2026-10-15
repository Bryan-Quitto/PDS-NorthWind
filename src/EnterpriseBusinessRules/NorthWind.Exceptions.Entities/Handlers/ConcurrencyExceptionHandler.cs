using NorthWind.Exceptions.Entities.Exceptions;
using NorthWind.Exceptions.Entities.Extensions;

namespace NorthWind.Exceptions.Entities.Handlers;

public class ConcurrencyExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ConcurrencyException concurrencyException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            Title = "Conflicto de concurrencia",
            Detail = concurrencyException.Message,
            Instance = httpContext.Request.Path
        };

        if (concurrencyException.Entries.Count > 0)
        {
            problemDetails.Extensions["entries"] = concurrencyException.Entries;
        }

        await httpContext.WriteProblemDetailsAsync(problemDetails, cancellationToken);
        return true;
    }
}
