using Authentication.API.DTOs.Auth.Requests;
using FluentValidation;

namespace Authentication.API.Validators;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address cannot be empty")
            .EmailAddress().WithMessage("Email address must be in a valid format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}
