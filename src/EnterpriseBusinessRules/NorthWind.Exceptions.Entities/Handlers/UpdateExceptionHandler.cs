using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Exceptions.Entities.Exceptions;
using NorthWind.Exceptions.Entities.Extensions;

namespace NorthWind.Exceptions.Entities.Handlers;

public class UpdateExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UpdateException updateException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = "Error de actualización de base de datos",
            Detail = updateException.Message,
            Instance = httpContext.Request.Path
        };

        if (updateException.Entries.Count > 0)
        {
            problemDetails.Extensions["entries"] = updateException.Entries;
        }

        await httpContext.WriteProblemDetailsAsync(problemDetails, cancellationToken);
        return true;
    }
}
