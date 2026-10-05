using NorthWind.Membership.Entities.Dtos;

namespace NorthWind.Membership.Backend.Core.Interfaces.Ports;

public interface IUserLoginInputPort
{
    Task HandleAsync(UserCredentialsDto credentialsDto, CancellationToken cancellationToken = default);
}
