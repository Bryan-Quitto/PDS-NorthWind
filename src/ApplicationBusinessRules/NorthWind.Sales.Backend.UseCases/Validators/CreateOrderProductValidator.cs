using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Enums;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Sales.Backend.UseCases.Validators;

public class CreateOrderProductValidator : AbstractModelValidator<CreateOrderDto>
{
    private readonly IQueriesRepository queriesRepository;

    public CreateOrderProductValidator(
        IValidationService<CreateOrderDto> validationService,
        IQueriesRepository queriesRepository)
        : base(validationService, ValidationConstraint.ValidateIfThereAreNoPreviousErrors)
    {
        this.queriesRepository = queriesRepository;
    }

    public override async Task<IReadOnlyList<ValidationError>> ValidateAsync(CreateOrderDto model, CancellationToken cancellationToken = default)
    {
        var baseErrors = await base.ValidateAsync(model, cancellationToken);
        var errors = new List<ValidationError>(baseErrors);

        if (model.OrderDetails == null || model.OrderDetails.Count == 0)
        {
            return errors;
        }

        var productIds = model.OrderDetails.Select(d => d.ProductId).Distinct().ToList();
        var stockList = await queriesRepository.GetProductsUnitsInStockAsync(productIds, cancellationToken);
        var stockMap = stockList.ToDictionary(s => s.ProductId, s => s.UnitsInStock);

        // Agrupación para validar suma consolidada de unidades requeridas por producto
        var groupedQuantities = model.OrderDetails
            .GroupBy(d => d.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(d => d.Quantity));

        for (int i = 0; i < model.OrderDetails.Count; i++)
        {
            var detail = model.OrderDetails[i];

            // 1. Validar existencia del producto
            if (!stockMap.TryGetValue(detail.ProductId, out var unitsInStock))
            {
                errors.Add(new ValidationError(
                    $"OrderDetails[{i}].ProductId",
                    $"El producto con ID {detail.ProductId} no existe en el catálogo."
                ));
            }
            // 2. Validar existencias acumuladas contra el inventario
            else if (groupedQuantities.TryGetValue(detail.ProductId, out var totalRequested) && totalRequested > unitsInStock)
            {
                errors.Add(new ValidationError(
                    $"OrderDetails[{i}].Quantity",
                    $"Las unidades solicitadas en la orden superan las existencias para el producto {detail.ProductId}. Existencias: {unitsInStock}, solicitadas en total: {totalRequested}."
                ));
            }
        }

        return errors;
    }
}
