using NorthWind.DomainLogs.Entities.ValueObjects;

namespace NorthWind.DomainLogs.Entities.Interfaces;

public interface IDomainLogsRepository
{
    Task AddAsync(DomainLog log, CancellationToken cancellationToken = default);
}
