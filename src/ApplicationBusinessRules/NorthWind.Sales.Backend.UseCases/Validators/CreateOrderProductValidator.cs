using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Sales.Backend.UseCases.Validators;

public class CreateOrderProductValidator : AbstractModelValidator<CreateOrderDto>
{
    public CreateOrderProductValidator(
        IValidationService<CreateOrderDto> validationService,
        IQueriesRepository queriesRepository)
        : base(validationService)
    {
        RuleFor(o => o.OrderDetails)
            .MustAsync(async (details, ct) =>
            {
                if (details == null || details.Count == 0)
                {
                    return true;
                }

                var productIds = details.Select(d => d.ProductId).Distinct().ToList();
                var stockList = await queriesRepository.GetProductsUnitsInStockAsync(productIds, ct);

                if (stockList.Count != productIds.Count)
                {
                    return false;
                }

                foreach (var detail in details)
                {
                    var stockItem = stockList.FirstOrDefault(s => s.ProductId == detail.ProductId);
                    if (stockItem.UnitsInStock < detail.Quantity)
                    {
                        return false;
                    }
                }

                return true;
            }, "Uno o más productos no existen o no cuentan con existencias suficientes en inventario.");
    }
}
