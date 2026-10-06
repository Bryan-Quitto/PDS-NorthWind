using NorthWind.Membership.Backend.Core.Dtos;

namespace NorthWind.Membership.Backend.Core.Interfaces.Services;

public interface IMembershipService
{
    Task<Result<IEnumerable<string>>> RegisterUserAsync(UserRegistrationDto user, CancellationToken cancellationToken = default);
    Task<Result<UserDto, IEnumerable<string>>> GetUserByCredentialsAsync(UserCredentialsDto credentials, CancellationToken cancellationToken = default);
}
