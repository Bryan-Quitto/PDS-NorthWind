namespace NorthWind.Membership.Backend.Core.Interfaces.Ports;

public interface IUserLoginOutputPort
{
    Task HandleAsync(Result<TokensDto, IEnumerable<string>> result, CancellationToken cancellationToken = default);
}
