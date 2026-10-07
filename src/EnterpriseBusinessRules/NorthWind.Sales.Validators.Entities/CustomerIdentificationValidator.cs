using NorthWind.Sales.Validators.Entities.Common;
using NorthWind.Sales.Validators.Entities.Resources;

namespace NorthWind.Sales.Validators.Entities;

public class CustomerIdentificationValidator : AbstractModelValidator<string>
{
    public CustomerIdentificationValidator(IValidationService<string> validationService)
        : base(validationService)
    {
        RuleFor(id => id)
            .NotEmpty(CreateOrderMessages.CustomerIdentificationRequired)
            .Length(10, CreateOrderMessages.CustomerIdentificationInvalid)
            .Must(id => EcuadorianIdentificationValidator.IsValid(id), CreateOrderMessages.CustomerIdentificationInvalid);
    }
}
