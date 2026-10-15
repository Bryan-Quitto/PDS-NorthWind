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
            var validationErrors = new List<ValidationError>();

            if (problemDetails?.Errors != null && problemDetails.Errors.Count > 0)
            {
                foreach (var (prop, messages) in problemDetails.Errors)
                {
                    foreach (var msg in messages)
                    {
                        validationErrors.Add(new ValidationError(prop, msg));
                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(problemDetails?.Detail))
            {
                validationErrors.Add(new ValidationError(string.Empty, problemDetails.Detail));
            }

            var httpRequestException = new HttpRequestException(
                problemDetails?.Detail ?? "Se presentaron errores de validación en la solicitud.",
                null,
                response.StatusCode);

            httpRequestException.Data["Errors"] = validationErrors;
            throw httpRequestException;
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>(JsonOptions, cancellationToken);
            var message = !string.IsNullOrWhiteSpace(problemDetails?.Detail) && !problemDetails.Detail.Contains("at System.", StringComparison.OrdinalIgnoreCase)
                ? problemDetails.Detail
                : "El servicio no se encuentra disponible, por favor contáctese con administración.";

            var ex = new HttpRequestException(
                message,
                null,
                response.StatusCode);

            if (problemDetails?.Entries != null && problemDetails.Entries.Count > 0)
            {
                ex.Data["Errors"] = problemDetails.Entries;
            }

            throw ex;
        }

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>(JsonOptions, cancellationToken);
            var detail = problemDetails?.Detail ?? "El registro está siendo modificado por otra transacción en curso. Por favor, espere a que finalice antes de intentar nuevamente.";
            var title = problemDetails?.Title ?? "Conflicto de concurrencia";
            var message = !string.IsNullOrWhiteSpace(problemDetails?.Detail)
                ? problemDetails.Detail
                : (!string.IsNullOrWhiteSpace(problemDetails?.Title) ? problemDetails.Title : "Conflicto de concurrencia detectado.");

            var ex = new HttpRequestException(
                message,
                null,
                response.StatusCode);

            if (problemDetails?.Entries != null && problemDetails.Entries.Count > 0)
            {
                ex.Data["Errors"] = problemDetails.Entries;
            }

            throw ex;
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
