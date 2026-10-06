using NorthWind.Validation.Entities.Enums;

namespace NorthWind.Sales.Backend.UseCases.Validators;

public class CreateOrderCustomerValidator : AbstractModelValidator<CreateOrderDto>
{
    public CreateOrderCustomerValidator(
        IValidationService<CreateOrderDto> validationService,
        IQueriesRepository queriesRepository)
        : base(validationService, ValidationConstraint.ValidateIfThereAreNoPreviousErrors)
    {
        RuleFor(o => o.CustomerId)
            .MustAsync(async (customerId, ct) =>
            {
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    return false;
                }
                return await queriesRepository.CustomerExistsAsync(customerId, ct);
            }, "El cliente especificado no existe en el sistema.")
            .MustAsync(async (customerId, ct) =>
            {
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    return true;
                }
                var balance = await queriesRepository.GetCustomerCurrentBalanceAsync(customerId, ct);
                return balance <= 0;
            }, "El cliente tiene un saldo pendiente y no se puede procesar la orden.");
    }
}
