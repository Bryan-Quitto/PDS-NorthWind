namespace NorthWind.Membership.Backend.Core.Interfaces.Ports;

public interface IUserRegistrationOutputPort
{
    Task HandleAsync(Result<IEnumerable<string>> result, CancellationToken cancellationToken = default);
}
