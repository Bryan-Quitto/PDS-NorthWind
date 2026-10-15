using NorthWind.Membership.Entities.Resources;
using NorthWind.Membership.Entities.Validators;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;

namespace NorthWind.Membership.Frontend.RazorViews.ViewModels;

public class UserRegistrationViewModelValidator : AbstractModelValidator<UserRegistrationViewModel>
{
    public UserRegistrationViewModelValidator(IValidationService<UserRegistrationViewModel> validationService)
        : base(validationService)
    {
        RuleFor(u => u.FirstName)
            .NotEmpty(MembershipMessages.FirstNameRequired)
            .Length(1, 100, MembershipMessages.FirstNameMaxLength)
            .Must(UserNameRulesValidator.IsValidFormat, MembershipMessages.FirstNameInvalidFormat);

        RuleFor(u => u.LastName)
            .NotEmpty(MembershipMessages.LastNameRequired)
            .Length(1, 100, MembershipMessages.LastNameMaxLength)
            .Must(UserNameRulesValidator.IsValidFormat, MembershipMessages.LastNameInvalidFormat);

        RuleFor(u => u.Email)
            .NotEmpty(MembershipMessages.EmailRequired)
            .Length(1, 256, MembershipMessages.EmailMaxLength)
            .Must(UserEmailRulesValidator.HasNoConsecutiveDots, MembershipMessages.EmailConsecutiveDots)
            .Must(UserEmailRulesValidator.IsValidFormat, MembershipMessages.EmailInvalid);

        RuleFor(u => u.Password)
            .NotEmpty(MembershipMessages.PasswordRequired)
            .Length(6, 100, MembershipMessages.PasswordTooShort)
            .Must(p => !string.IsNullOrEmpty(p) && p.Any(char.IsLower), MembershipMessages.PasswordRequiresLower)
            .Must(p => !string.IsNullOrEmpty(p) && p.Any(char.IsUpper), MembershipMessages.PasswordRequiresUpper)
            .Must(p => !string.IsNullOrEmpty(p) && p.Any(char.IsDigit), MembershipMessages.PasswordRequiresDigit)
            .Must(p => !string.IsNullOrEmpty(p) && p.Any(ch => !char.IsLetterOrDigit(ch)), MembershipMessages.PasswordRequiresNonAlphanumeric);

        RuleFor(u => u.PasswordConfirmation)
            .NotEmpty("La confirmación de la contraseña es requerida.")
            .Equal(u => u.Password, "La contraseña y la confirmación no coinciden.");
    }
}
