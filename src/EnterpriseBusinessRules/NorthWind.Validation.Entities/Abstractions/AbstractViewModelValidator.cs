using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Validation.Entities.Abstractions;

public abstract class AbstractViewModelValidator<TDto, TViewModel>(IValidationService<TViewModel> validationService)
    : AbstractModelValidator<TViewModel>(validationService)
{
}
