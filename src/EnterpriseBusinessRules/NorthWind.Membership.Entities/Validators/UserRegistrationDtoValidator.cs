using NorthWind.Membership.Entities.Dtos;
using NorthWind.Membership.Entities.Resources;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Membership.Entities.Validators;

public class UserRegistrationDtoValidator : IModelValidator<UserRegistrationDto>
{
    public Task<IReadOnlyList<ValidationError>> ValidateAsync(UserRegistrationDto model, CancellationToken cancellationToken = default)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(model.FirstName))
        {
            errors.Add(new ValidationError(nameof(model.FirstName), MembershipMessages.FirstNameRequired));
        }

        if (string.IsNullOrWhiteSpace(model.LastName))
        {
            errors.Add(new ValidationError(nameof(model.LastName), MembershipMessages.LastNameRequired));
        }

        if (string.IsNullOrWhiteSpace(model.Email))
        {
            errors.Add(new ValidationError(nameof(model.Email), MembershipMessages.EmailRequired));
        }
        else if (!model.Email.Contains('@') || !model.Email.Contains('.'))
        {
            errors.Add(new ValidationError(nameof(model.Email), MembershipMessages.EmailInvalid));
        }

        if (string.IsNullOrWhiteSpace(model.Password))
        {
            errors.Add(new ValidationError(nameof(model.Password), MembershipMessages.PasswordRequired));
        }
        else
        {
            if (model.Password.Length < 6)
            {
                errors.Add(new ValidationError(nameof(model.Password), MembershipMessages.PasswordTooShort));
            }

            if (!model.Password.Any(char.IsLower))
            {
                errors.Add(new ValidationError(nameof(model.Password), MembershipMessages.PasswordRequiresLower));
            }

            if (!model.Password.Any(char.IsUpper))
            {
                errors.Add(new ValidationError(nameof(model.Password), MembershipMessages.PasswordRequiresUpper));
            }

            if (!model.Password.Any(char.IsDigit))
            {
                errors.Add(new ValidationError(nameof(model.Password), MembershipMessages.PasswordRequiresDigit));
            }

            if (!model.Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                errors.Add(new ValidationError(nameof(model.Password), MembershipMessages.PasswordRequiresNonAlphanumeric));
            }
        }

        return Task.FromResult<IReadOnlyList<ValidationError>>(errors);
    }
}
