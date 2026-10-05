using NorthWind.Membership.Backend.Core.Interfaces.Ports;
using NorthWind.Membership.Backend.Core.Interfaces.Services;
using NorthWind.Membership.Entities.Dtos;
using NorthWind.Result.Entities;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Membership.Backend.Core.UseCases;

public class UserRegistrationInteractor(
    IModelValidator<UserRegistrationDto> validator,
    IMembershipService membershipService,
    IUserRegistrationOutputPort outputPort) : IUserRegistrationInputPort
{
    public async Task HandleAsync(UserRegistrationDto registrationDto, CancellationToken cancellationToken = default)
    {
        var validationErrors = await validator.ValidateAsync(registrationDto, cancellationToken);
        if (validationErrors.Count > 0)
        {
            var errors = validationErrors.Select(e => e.ErrorMessage);
            await outputPort.HandleAsync(Result<IEnumerable<string>>.Fail(errors), cancellationToken);
            return;
        }

        var result = await membershipService.RegisterUserAsync(registrationDto, cancellationToken);
        await outputPort.HandleAsync(result, cancellationToken);
    }
}
