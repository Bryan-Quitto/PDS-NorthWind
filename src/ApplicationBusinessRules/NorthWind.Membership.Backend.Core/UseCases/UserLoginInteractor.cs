using NorthWind.Membership.Backend.Core.Interfaces.Ports;
using NorthWind.Membership.Backend.Core.Interfaces.Services;
using NorthWind.Membership.Backend.Core.Services;
using NorthWind.Membership.Entities.Dtos;
using NorthWind.Result.Entities;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Membership.Backend.Core.UseCases;

public class UserLoginInteractor(
    IModelValidator<UserCredentialsDto> validator,
    IMembershipService membershipService,
    JwtService jwtService,
    IUserLoginOutputPort outputPort) : IUserLoginInputPort
{
    public async Task HandleAsync(UserCredentialsDto credentialsDto, CancellationToken cancellationToken = default)
    {
        var validationErrors = await validator.ValidateAsync(credentialsDto, cancellationToken);
        if (validationErrors.Count > 0)
        {
            var errors = validationErrors.Select(e => e.ErrorMessage);
            await outputPort.HandleAsync(Result<TokensDto, IEnumerable<string>>.Fail(errors), cancellationToken);
            return;
        }

        var result = await membershipService.GetUserByCredentialsAsync(credentialsDto, cancellationToken);
        if (result.HasError)
        {
            await outputPort.HandleAsync(Result<TokensDto, IEnumerable<string>>.Fail(result.ErrorValue!), cancellationToken);
            return;
        }

        var tokenString = jwtService.GenerateToken(result.SuccessValue!);
        var tokensDto = new TokensDto(tokenString);

        await outputPort.HandleAsync(Result<TokensDto, IEnumerable<string>>.Success(tokensDto), cancellationToken);
    }
}
