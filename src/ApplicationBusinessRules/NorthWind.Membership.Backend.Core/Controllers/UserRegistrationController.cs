using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Membership.Backend.Core.Interfaces.Ports;
using NorthWind.Membership.Backend.Core.Presenters;
using NorthWind.Membership.Entities.Constants;
using NorthWind.Membership.Entities.Dtos;

namespace NorthWind.Membership.Backend.Core.Controllers;

public static class UserRegistrationController
{
    public static WebApplication UseUserRegistrationController(this WebApplication app)
    {
        app.MapPost(Endpoints.Register, async (
            [FromBody] UserRegistrationDto registrationDto,
            IUserRegistrationInputPort inputPort,
            UserRegistrationPresenter presenter,
            CancellationToken cancellationToken) =>
        {
            await inputPort.HandleAsync(registrationDto, cancellationToken);
            return presenter.Result;
        })
        .WithName("RegisterUser")
        .WithTags("Membership")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        return app;
    }
}
