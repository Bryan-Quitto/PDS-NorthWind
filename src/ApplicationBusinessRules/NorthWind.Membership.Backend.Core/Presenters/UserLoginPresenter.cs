using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Membership.Backend.Core.Interfaces.Ports;
using NorthWind.Membership.Entities.Dtos;
using NorthWind.Result.Entities;

namespace NorthWind.Membership.Backend.Core.Presenters;

public class UserLoginPresenter : IUserLoginOutputPort
{
    public IResult Result { get; private set; } = Results.BadRequest();

    public Task HandleAsync(Result<TokensDto, IEnumerable<string>> result, CancellationToken cancellationToken = default)
    {
        Result = result.HandleResult(
            tokens => Results.Ok(tokens),
            errors =>
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                    Title = "Error en el inicio de sesión",
                    Detail = "Uno o más errores de validación o autenticación ocurrieron."
                };
                problemDetails.Extensions["errors"] = errors;
                return Results.Problem(problemDetails);
            }
        );

        return Task.CompletedTask;
    }
}
