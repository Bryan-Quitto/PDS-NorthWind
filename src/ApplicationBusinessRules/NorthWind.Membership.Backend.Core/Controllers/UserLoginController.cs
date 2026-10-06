using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Membership.Backend.Core.Interfaces.Ports;
using NorthWind.Membership.Backend.Core.Presenters;
using NorthWind.Membership.Entities.Constants;

namespace NorthWind.Membership.Backend.Core.Controllers;

public static class UserLoginController
{
    public static WebApplication UseUserLoginController(this WebApplication app)
    {
        app.MapPost(Endpoints.Login, async (
            [FromBody] UserCredentialsDto credentialsDto,
            IUserLoginInputPort inputPort,
            UserLoginPresenter presenter,
            CancellationToken cancellationToken) =>
        {
            await inputPort.HandleAsync(credentialsDto, cancellationToken);
            return presenter.Result;
        })
        .WithName("LoginUser")
        .WithTags("Membership")
        .Produces<TokensDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        return app;
    }
}
