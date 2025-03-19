using Authentication.API.DTOs.Auth.Requests;
using FluentValidation;
using static Authentication.API.LocalizationKeys;

namespace Authentication.API.Validators;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(EmailCannotBeEmpty)
            .EmailAddress().WithMessage(EmailMustBeInValidFormat);
    }
}
