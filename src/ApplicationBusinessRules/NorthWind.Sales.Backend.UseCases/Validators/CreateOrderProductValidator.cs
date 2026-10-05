using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Sales.Backend.UseCases.Validators;

public class CreateOrderProductValidator : AbstractModelValidator<CreateOrderDto>
{
    private readonly IQueriesRepository queriesRepository;

    public CreateOrderProductValidator(
        IValidationService<CreateOrderDto> validationService,
        IQueriesRepository queriesRepository)
        : base(validationService)
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

        for (int i = 0; i < model.OrderDetails.Count; i++)
        {
            var detail = model.OrderDetails[i];
            if (!stockMap.TryGetValue(detail.ProductId, out var unitsInStock))
            {
                errors.Add(new ValidationError(
                    $"OrderDetails[{i}].ProductId",
                    $"El producto con ID {detail.ProductId} no existe en el catálogo."
                ));
            }
            else if (unitsInStock < detail.Quantity)
            {
                errors.Add(new ValidationError(
                    $"OrderDetails[{i}].Quantity",
                    $"No hay existencias suficientes para el producto con ID {detail.ProductId}. Existencias actuales: {unitsInStock}, solicitadas: {detail.Quantity}."
                ));
            }
        }

        return errors;
    }
}
