using NorthWind.Sales.Backend.Repositories.Interfaces;

namespace NorthWind.Sales.Backend.Repositories.Repositories;

public class DomainLogsRepository(INorthWindDomainLogsDataContext context) : IDomainLogsRepository
{
    public async Task AddAsync(DomainLog log, CancellationToken cancellationToken = default)
    {
        await context.AddLogAsync(log, cancellationToken);
    }
}
