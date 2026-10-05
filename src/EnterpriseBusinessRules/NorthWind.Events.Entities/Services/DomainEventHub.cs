using NorthWind.Events.Entities.Interfaces;

namespace NorthWind.Events.Entities.Services;

public class DomainEventHub<TEvent>(IEnumerable<IDomainEventHandler<TEvent>> handlers) : IDomainEventHub<TEvent>
    where TEvent : IDomainEvent
{
    public async Task RaiseAsync(TEvent domainEvent, CancellationToken cancellationToken = default)
    {
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(domainEvent, cancellationToken);
        }
    }
}
