using NorthWind.DomainLogs.Entities.Interfaces;
using NorthWind.DomainLogs.Entities.ValueObjects;

namespace NorthWind.DomainLogs.Entities.Services;

public class DomainLogger(IDomainLogsRepository repository) : IDomainLogger
{
    public async Task LogInformationAsync(DomainLog log, CancellationToken cancellationToken = default)
    {
        await repository.AddAsync(log, cancellationToken);
    }
}
