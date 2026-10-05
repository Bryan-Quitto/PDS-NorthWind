using NorthWind.Entities;
using NorthWind.Events.Entities.Interfaces;
using NorthWind.Sales.Backend.UseCases.Events;

namespace NorthWind.Sales.Backend.UseCases.EventHandlers;

public class SendEMailWhenSpecialOrderCreatedEventHandler(IMailService mailService)
    : IDomainEventHandler<SpecialOrderCreatedEvent>
{
    public async Task HandleAsync(SpecialOrderCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        string to = "admin@northwind.com";
        string subject = $"Orden especial creada: {domainEvent.OrderId}";
        string body = $"Se ha generado exitosamente la orden especial número {domainEvent.OrderId} con {domainEvent.OrderDetailsCount} partidas de productos el {domainEvent.OccurredOn:yyyy-MM-dd HH:mm:ss} UTC.";

        await mailService.SendEmailAsync(to, subject, body, cancellationToken);
    }
}
