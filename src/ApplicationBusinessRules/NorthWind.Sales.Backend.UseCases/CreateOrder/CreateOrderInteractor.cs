using NorthWind.DomainLogs.Entities.Interfaces;
using NorthWind.DomainLogs.Entities.ValueObjects;
using NorthWind.Entities;
using NorthWind.Events.Entities.Interfaces;
using NorthWind.Sales.Backend.BusinessObjects.Aggregates;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.BusinessObjects.Services;
using NorthWind.Sales.Backend.UseCases.Events;
using NorthWind.Sales.Entities.Dtos;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

public class CreateOrderInteractor(
    IModelValidatorHub<CreateOrderDto> validatorHub,
    ICommandsRepository commandsRepository,
    IUnitOfWork unitOfWork,
    IDomainLogger domainLogger,
    IDomainEventHub<SpecialOrderCreatedEvent> eventHub,
    ICreateOrderOutputPort outputPort) : ICreateOrderInputPort
{
    public async Task HandleAsync(CreateOrderDto orderDto, CancellationToken cancellationToken = default)
    {
        // 1. Validaciones agnósticas (formato y reglas de negocio contra BD)
        await GuardModel.AgainstErrorsAsync(validatorHub, orderDto, cancellationToken);

        // 2. Auditoría inicial de dominio
        await domainLogger.LogInformationAsync(new DomainLog(
            action: "CreateOrder.Start",
            description: $"Iniciando creación de orden para el cliente '{orderDto.CustomerId}' con {orderDto.OrderDetails.Count} partidas.",
            user: "System"
        ), cancellationToken);

        // 3. Crear el agregado de dominio y consolidar partidas duplicadas si existieran
        var orderAggregate = OrderAggregate.From(orderDto);

        // 4. Persistir a través del repositorio de comandos
        await commandsRepository.CreateOrderAsync(orderAggregate, cancellationToken);

        // 5. Commit atómico vía Unit of Work
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Auditoría final de dominio
        await domainLogger.LogInformationAsync(new DomainLog(
            action: "CreateOrder.Completed",
            description: $"Orden {orderAggregate.Id} creada y persistida exitosamente para el cliente '{orderAggregate.CustomerId}'.",
            user: "System"
        ), cancellationToken);

        // 7. Notificar evento de dominio si se trata de una orden especial (> 3 partidas)
        if (orderAggregate.OrderDetails.Count > 3)
        {
            await eventHub.RaiseAsync(
                new SpecialOrderCreatedEvent(orderAggregate.Id, orderAggregate.OrderDetails.Count),
                cancellationToken
            );
        }

        // 8. Notificar el ID generado a la capa de presentación mediante el Output Port
        await outputPort.HandleAsync(orderAggregate.Id, cancellationToken);
    }
}
