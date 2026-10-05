using NorthWind.DomainLogs.Entities.ValueObjects;

namespace NorthWind.DomainLogs.Entities.Interfaces;

public interface IDomainLogger
{
    Task LogInformationAsync(DomainLog log, CancellationToken cancellationToken = default);
}
