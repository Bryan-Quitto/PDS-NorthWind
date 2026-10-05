using NorthWind.Events.Entities.Interfaces;

namespace NorthWind.Sales.Backend.UseCases.Events;

public record SpecialOrderCreatedEvent(int OrderId, int OrderDetailsCount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
