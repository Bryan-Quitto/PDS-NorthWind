using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthWind.Exceptions.Entities.Extensions;

namespace NorthWind.Exceptions.Entities.Handlers;

public class UnhandledExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = "El servicio no se encuentra disponible, por favor contáctese con administración.",
            Instance = httpContext.Request.Path
        };

        await httpContext.WriteProblemDetailsAsync(problemDetails, cancellationToken);
        return true;
    }
}
