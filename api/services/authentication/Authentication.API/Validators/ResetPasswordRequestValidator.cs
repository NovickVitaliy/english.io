using Authentication.API.DTOs.Other;
using FluentValidation;

namespace Authentication.API.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address cannot be empty")
            .EmailAddress().WithMessage("Email address must be in a valid format");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("'New Password' cannot be empty");

        RuleFor(x => x.NewPasswordConfirm)
            .NotEmpty().WithMessage("'New Confirm Password' cannot be empty")
            .Equal(x => x.NewPassword).WithMessage("'New Password' and 'New Confirm Password' must match");

        RuleFor(x => x.ResetToken)
            .NotEmpty().WithMessage("Reset token cannot be empty");
    }
}
