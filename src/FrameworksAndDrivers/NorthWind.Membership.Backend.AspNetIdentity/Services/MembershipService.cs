using Microsoft.AspNetCore.Identity;
using NorthWind.Membership.Backend.AspNetIdentity.Entities;
using NorthWind.Membership.Backend.Core.Dtos;
using NorthWind.Membership.Backend.Core.Interfaces.Services;
using NorthWind.Membership.Entities.Dtos;
using NorthWind.Membership.Entities.Resources;
using NorthWind.Result.Entities;

namespace NorthWind.Membership.Backend.AspNetIdentity.Services;

public class MembershipService(UserManager<NorthWindUser> userManager) : IMembershipService
{
    public async Task<Result<IEnumerable<string>>> RegisterUserAsync(
        UserRegistrationDto user,
        CancellationToken cancellationToken = default)
    {
        var identityUser = new NorthWindUser
        {
            UserName = user.Email,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        var identityResult = await userManager.CreateAsync(identityUser, user.Password);
        if (identityResult.Succeeded)
        {
            return Result<IEnumerable<string>>.Success();
        }

        var errors = identityResult.Errors.Select(e => e.Description);
        return Result<IEnumerable<string>>.Fail(errors);
    }

    public async Task<Result<UserDto, IEnumerable<string>>> GetUserByCredentialsAsync(
        UserCredentialsDto credentials,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(credentials.Email)
                   ?? await userManager.FindByNameAsync(credentials.Email);

        if (user == null)
        {
            return Result<UserDto, IEnumerable<string>>.Fail([MembershipMessages.InvalidCredentials]);
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, credentials.Password);
        if (!isPasswordValid)
        {
            return Result<UserDto, IEnumerable<string>>.Fail([MembershipMessages.InvalidCredentials]);
        }

        var userDto = new UserDto(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            $"{user.FirstName} {user.LastName}".Trim()
        );

        return Result<UserDto, IEnumerable<string>>.Success(userDto);
    }
}
