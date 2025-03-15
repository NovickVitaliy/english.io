using Authentication.API.DTOs.Auth.Requests;
using FluentValidation;
using static Authentication.API.LocalizationKeys;

namespace Authentication.API.Validators;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(EmailCannotBeEmpty)
            .EmailAddress().WithMessage(EmailMustBeInValidFormat);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(PasswordCannotBeEmpty);
    }
}
