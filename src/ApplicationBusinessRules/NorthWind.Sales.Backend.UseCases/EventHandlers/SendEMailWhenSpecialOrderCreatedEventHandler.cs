using NorthWind.Entities;
using NorthWind.Events.Entities.Interfaces;
using NorthWind.Sales.Backend.UseCases.Events;

namespace NorthWind.Sales.Backend.UseCases.EventHandlers;

public class SendEMailWhenSpecialOrderCreatedEventHandler(IMailService mailService)
    : IDomainEventHandler<SpecialOrderCreatedEvent>
{
    public async Task HandleAsync(SpecialOrderCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        string subject = "Orden especial creada";
        string body = $"Se ha creado la orden {domainEvent.OrderId} con {domainEvent.OrderDetailsCount} productos.";

        await mailService.SendMailToAdministrator(subject, body, cancellationToken);
    }
}
