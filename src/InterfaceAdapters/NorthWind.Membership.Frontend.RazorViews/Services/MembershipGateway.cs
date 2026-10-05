using System.Text.Json;
using NorthWind.Membership.Entities.Constants;
using NorthWind.Membership.Entities.Dtos;
using NorthWind.Result.Entities;

namespace NorthWind.Membership.Frontend.RazorViews.Services;

public class MembershipGateway(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<Result<IEnumerable<string>>> RegisterAsync(
        UserRegistrationDto user,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(Endpoints.Register, user, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return Result<IEnumerable<string>>.Success();
        }

        var errors = await ExtractErrorsFromResponseAsync(response, cancellationToken);
        return Result<IEnumerable<string>>.Fail(errors);
    }

    public async Task<Result<TokensDto, IEnumerable<string>>> LoginAsync(
        UserCredentialsDto credentials,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(Endpoints.Login, credentials, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var tokens = await response.Content.ReadFromJsonAsync<TokensDto>(JsonOptions, cancellationToken);
            if (tokens != null)
            {
                return Result<TokensDto, IEnumerable<string>>.Success(tokens);
            }
        }

        var errors = await ExtractErrorsFromResponseAsync(response, cancellationToken);
        return Result<TokensDto, IEnumerable<string>>.Fail(errors);
    }

    private static async Task<List<string>> ExtractErrorsFromResponseAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        List<string> errorsList = [];
        try
        {
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            // 1. Extraer del arreglo o lista "errors" (ProblemDetails estándar de ASP.NET Core)
            if (doc.RootElement.TryGetProperty("errors", out var errorsProp))
            {
                if (errorsProp.ValueKind == JsonValueKind.Array)
                {
                    errorsList = errorsProp.EnumerateArray()
                                           .Select(e => e.GetString() ?? string.Empty)
                                           .Where(s => !string.IsNullOrWhiteSpace(s))
                                           .ToList();
                }
                else if (errorsProp.ValueKind == JsonValueKind.Object)
                {
                    // Errores de validación estándar agrupados por campo: { "Field": ["msg1", "msg2"] }
                    foreach (var property in errorsProp.EnumerateObject())
                    {
                        if (property.Value.ValueKind == JsonValueKind.Array)
                        {
                            errorsList.AddRange(property.Value.EnumerateArray()
                                                              .Select(e => e.GetString() ?? string.Empty)
                                                              .Where(s => !string.IsNullOrWhiteSpace(s)));
                        }
                    }
                }
            }

            // 2. Extraer de "detail" si no hubo arreglo de errores
            if (errorsList.Count == 0 && doc.RootElement.TryGetProperty("detail", out var detailProp) && detailProp.ValueKind == JsonValueKind.String)
            {
                var detail = detailProp.GetString();
                if (!string.IsNullOrWhiteSpace(detail))
                {
                    errorsList.Add(detail);
                }
            }

            // 3. Extraer de "title" como último recurso
            if (errorsList.Count == 0 && doc.RootElement.TryGetProperty("title", out var titleProp) && titleProp.ValueKind == JsonValueKind.String)
            {
                var title = titleProp.GetString();
                if (!string.IsNullOrWhiteSpace(title))
                {
                    errorsList.Add(title);
                }
            }
        }
        catch
        {
            // Fallback si la respuesta no es un JSON válido
            errorsList.Add("Error al procesar la respuesta del servidor.");
        }

        return errorsList.Count > 0 ? errorsList : ["Error en la solicitud."];
    }
}
