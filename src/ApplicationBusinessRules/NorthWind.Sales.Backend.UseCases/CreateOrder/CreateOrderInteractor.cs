using NorthWind.Sales.Backend.BusinessObjects.Specifications;
using NorthWind.Sales.Backend.UseCases.Events;
using NorthWind.Transactions.Entities.Interfaces;

namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

public class CreateOrderInteractor(
    IUserService userService,
    IModelValidatorHub<CreateOrderDto> validatorHub,
    ICommandsRepository commandsRepository,
    IUnitOfWork unitOfWork,
    IDomainLogger domainLogger,
    IDomainEventHub<SpecialOrderCreatedEvent> eventHub,
    IDomainTransaction domainTransaction,
    ICreateOrderOutputPort outputPort) : ICreateOrderInputPort
{
    public async Task HandleAsync(CreateOrderDto orderDto, CancellationToken cancellationToken = default)
    {
        // 1. Guard de autenticación de usuario
        GuardUser.AgainstUnauthenticated(userService);

        // 2. Validaciones agnósticas (formato, existencia y adeudo de cliente, inventario indexado)
        await GuardModel.AgainstErrorsAsync(validatorHub, orderDto, cancellationToken);

        // 3. Auditoría inicial de dominio
        await domainLogger.LogInformationAsync(new DomainLog(
            $"Iniciando creacion de orden para el cliente '{orderDto.CustomerId}' con {orderDto.OrderDetails.Count} partidas.",
            userService.UserName
        ), cancellationToken);

        // 4. Crear el agregado de dominio y consolidar partidas duplicadas
        var orderAggregate = OrderAggregate.From(orderDto);

        // 5. Manejo transaccional a nivel de caso de uso
        domainTransaction.BeginTransaction();
        try
        {
            // 6. Obtener las entidades de productos reales a rastrear por el contexto de comandos
            var productIds = orderAggregate.OrderDetails.Select(d => d.ProductId).Distinct().ToList();
            var trackedProducts = await commandsRepository.GetProductsByIdsAsync(productIds, cancellationToken);
            var productMap = trackedProducts.ToDictionary(p => p.Id);

            // 7. Verificar stock suficiente y descontar unidades en inventario
            var consolidatedQuantities = orderAggregate.OrderDetails
                .GroupBy(d => d.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(d => d.Quantity));

            foreach (var (productId, requestedQuantity) in consolidatedQuantities)
            {
                if (!productMap.TryGetValue(productId, out var trackedProduct))
                {
                    throw new NorthWind.Exceptions.Entities.Exceptions.ValidationException(
                        "OrderDetails",
                        $"El producto con ID {productId} no existe en el catálogo."
                    );
                }

                if (trackedProduct.UnitsInStock < requestedQuantity)
                {
                    throw new NorthWind.Exceptions.Entities.Exceptions.ValidationException(
                        "OrderDetails",
                        $"Stock insuficiente para el producto {trackedProduct.Name} (ID: {productId}). Existencias: {trackedProduct.UnitsInStock}, Solicitadas: {requestedQuantity}."
                    );
                }

                trackedProduct.UnitsInStock = (short)(trackedProduct.UnitsInStock - requestedQuantity);
            }

            // 8. Actualizar productos en el repositorio de comandos
            commandsRepository.UpdateProducts(trackedProducts);

            // 9. Persistir a través del repositorio de comandos
            await commandsRepository.CreateOrderAsync(orderAggregate, cancellationToken);

            // 10. Commit atómico vía Unit of Work (valida RowVersion con concurrencia optimista)
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 8. Auditoría final de dominio
            await domainLogger.LogInformationAsync(new DomainLog(
                $"Orden {orderAggregate.Id} creada y persistida exitosamente para el cliente '{orderAggregate.CustomerId}'.",
                userService.UserName
            ), cancellationToken);

            // 9. Notificar evento de dominio si se trata de una orden especial mediante el patrón Specification
            if (new SpecialOrderSpecification().IsSatisfiedBy(orderAggregate))
            {
                await eventHub.RaiseAsync(
                    new SpecialOrderCreatedEvent(orderAggregate.Id, orderAggregate.OrderDetails.Count),
                    cancellationToken
                );
            }

            // 10. Confirmar transacción de dominio
            domainTransaction.CommitTransaction();
        }
        catch (Exception)
        {
            domainTransaction.RollbackTransaction();
            await domainLogger.LogInformationAsync(new DomainLog(
                $"Creación de orden {orderAggregate.Id} cancelada.",
                userService.UserName
            ), cancellationToken);
            throw;
        }

        // 11. Notificar el ID generado a la capa de presentación mediante el Output Port
        await outputPort.HandleAsync(orderAggregate.Id, cancellationToken);
    }
}
