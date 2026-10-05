using NorthWind.Membership.Entities.Dtos;

namespace NorthWind.Membership.Backend.Core.Interfaces.Ports;

public interface IUserRegistrationInputPort
{
    Task HandleAsync(UserRegistrationDto registrationDto, CancellationToken cancellationToken = default);
}
