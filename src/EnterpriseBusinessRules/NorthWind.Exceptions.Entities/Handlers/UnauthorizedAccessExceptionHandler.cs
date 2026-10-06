using NorthWind.Exceptions.Entities.Extensions;

namespace NorthWind.Exceptions.Entities.Handlers;

public class UnauthorizedAccessExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UnauthorizedAccessException unauthorizedAccessException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
            Title = "No autorizado",
            Detail = unauthorizedAccessException.Message,
            Instance = httpContext.Request.Path
        };

        await httpContext.WriteProblemDetailsAsync(problemDetails, cancellationToken);
        return true;
    }
}
