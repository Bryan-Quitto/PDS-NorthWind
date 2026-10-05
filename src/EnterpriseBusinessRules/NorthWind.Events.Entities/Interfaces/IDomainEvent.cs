namespace NorthWind.Events.Entities.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
