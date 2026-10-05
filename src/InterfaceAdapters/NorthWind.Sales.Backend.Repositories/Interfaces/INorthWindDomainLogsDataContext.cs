using NorthWind.DomainLogs.Entities.ValueObjects;

namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindDomainLogsDataContext
{
    Task AddLogAsync(DomainLog log, CancellationToken cancellationToken = default);
}
