using NorthWind.Membership.Entities.Resources;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Membership.Frontend.RazorViews.ViewModels;

public class UserLoginViewModelValidator : AbstractModelValidator<UserLoginViewModel>
{
    public UserLoginViewModelValidator(IValidationService<UserLoginViewModel> validationService)
        : base(validationService)
    {
        RuleFor(u => u.Email)
            .NotEmpty(MembershipMessages.EmailRequired)
            .Must(email => !string.IsNullOrWhiteSpace(email) && email.Contains('@') && email.Contains('.'), MembershipMessages.EmailInvalid);

        RuleFor(u => u.Password)
            .NotEmpty(MembershipMessages.PasswordRequired);
    }
}
