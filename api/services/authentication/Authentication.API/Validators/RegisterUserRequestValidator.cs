using Authentication.API.DTOs.Auth.Requests;
using FluentValidation;

namespace Authentication.API.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address cannot be empty")
            .EmailAddress().WithMessage("Email address must be in a valid format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password cannot be empty")
            .Equal(x => x.Password).WithMessage("'Password' and 'Confirm Password' must match");
    }
}
