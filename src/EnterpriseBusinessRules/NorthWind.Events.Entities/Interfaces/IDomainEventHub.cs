namespace NorthWind.Events.Entities.Interfaces;

public interface IDomainEventHub<in TEvent> where TEvent : IDomainEvent
{
    Task RaiseAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
