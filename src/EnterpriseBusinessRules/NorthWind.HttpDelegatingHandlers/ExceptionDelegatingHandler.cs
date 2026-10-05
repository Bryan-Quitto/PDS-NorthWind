using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using NorthWind.Exceptions.Entities.Exceptions;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.HttpDelegatingHandlers;

public class ExceptionDelegatingHandler : DelegatingHandler
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>(JsonOptions, cancellationToken);
            if (problemDetails?.Errors != null && problemDetails.Errors.Count > 0)
            {
                var validationErrors = new List<ValidationError>();
                foreach (var (prop, messages) in problemDetails.Errors)
                {
                    foreach (var msg in messages)
                    {
                        validationErrors.Add(new ValidationError(prop, msg));
                    }
                }
                throw new ValidationException(validationErrors);
            }

            throw new ValidationException(problemDetails?.Detail ?? "Error de validación en la solicitud.");
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>(JsonOptions, cancellationToken);
            if (problemDetails?.Entries != null && problemDetails.Entries.Count > 0)
            {
                throw new UpdateException(problemDetails.Detail ?? "Error al actualizar la base de datos.", problemDetails.Entries);
            }

            throw new HttpRequestException(problemDetails?.Detail ?? "Error interno del servidor.", null, response.StatusCode);
        }

        response.EnsureSuccessStatusCode();
        return response;
    }

    private sealed class ProblemDetailsDto
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int? Status { get; set; }
        public string? Detail { get; set; }
        public string? Instance { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
        public List<string>? Entries { get; set; }
    }
}
