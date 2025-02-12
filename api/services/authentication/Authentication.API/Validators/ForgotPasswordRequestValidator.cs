using Authentication.API.DTOs.Auth.Requests;
using FluentValidation;

namespace Authentication.API.Validators;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address cannot be empty")
            .EmailAddress().WithMessage("Email address must be in a valid format");
    }
}
