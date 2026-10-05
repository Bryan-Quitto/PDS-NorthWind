using NorthWind.Membership.Entities.Dtos;
using NorthWind.Membership.Entities.Resources;
using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Membership.Entities.Validators;

public class UserCredentialsDtoValidator : IModelValidator<UserCredentialsDto>
{
    public Task<IReadOnlyList<ValidationError>> ValidateAsync(UserCredentialsDto model, CancellationToken cancellationToken = default)
    {
        var errors = new List<ValidationError>();

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

        return Task.FromResult<IReadOnlyList<ValidationError>>(errors);
    }
}
