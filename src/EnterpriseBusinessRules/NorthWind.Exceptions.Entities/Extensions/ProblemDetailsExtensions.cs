using System.Text.Json;

namespace NorthWind.Exceptions.Entities.Extensions;

public static class ProblemDetailsExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public static async Task WriteProblemDetailsAsync(
        this HttpContext context,
        ProblemDetails problemDetails,
        CancellationToken cancellationToken = default)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails, JsonOptions, cancellationToken);
    }
}
