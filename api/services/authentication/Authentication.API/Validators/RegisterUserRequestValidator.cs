using Authentication.API.DTOs.Auth.Requests;
using FluentValidation;
using static Authentication.API.LocalizationKeys;
namespace Authentication.API.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(EmailCannotBeEmpty)
            .EmailAddress().WithMessage(EmailMustBeInValidFormat);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(PasswordCannotBeEmpty);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(ConfirmPasswordCannotBeEmpty)
            .Equal(x => x.Password).WithMessage(PasswordAndConfirmPasswordMustMatch);
    }
}
