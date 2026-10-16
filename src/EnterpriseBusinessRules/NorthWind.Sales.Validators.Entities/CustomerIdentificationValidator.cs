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
            .Must(id => id != null && (id.Length == 10 || id.Length == 13), CreateOrderMessages.CustomerIdentificationInvalid)
            .Must(id => EcuadorianIdentificationValidator.IsValid(id), CreateOrderMessages.CustomerIdentificationInvalid);
    }
}
