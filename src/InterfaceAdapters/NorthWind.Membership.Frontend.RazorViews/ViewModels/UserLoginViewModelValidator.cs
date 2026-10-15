using NorthWind.Membership.Entities.Resources;
using NorthWind.Membership.Entities.Validators;
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
            .Length(1, 256, MembershipMessages.EmailMaxLength)
            .Must(UserEmailRulesValidator.HasNoConsecutiveDots, MembershipMessages.EmailConsecutiveDots)
            .Must(UserEmailRulesValidator.IsValidFormat, MembershipMessages.EmailInvalid);

        RuleFor(u => u.Password)
            .NotEmpty(MembershipMessages.PasswordRequired)
            .Length(1, 100, MembershipMessages.PasswordMaxLength);
    }
}
