using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Membership.Backend.Core.Interfaces.Ports;

namespace NorthWind.Membership.Backend.Core.Presenters;

public class UserRegistrationPresenter : IUserRegistrationOutputPort
{
    public IResult Result { get; private set; } = Results.Ok();

    public Task HandleAsync(Result<IEnumerable<string>> result, CancellationToken cancellationToken = default)
    {
        Result = result.HandleResult(
            () => Results.Ok(),
            errors =>
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                    Title = "Error en el registro del usuario",
                    Detail = "Uno o más errores de validación o registro ocurrieron."
                };
                problemDetails.Extensions["errors"] = errors;
                return Results.Problem(problemDetails);
            }
        );

        return Task.CompletedTask;
    }
}
