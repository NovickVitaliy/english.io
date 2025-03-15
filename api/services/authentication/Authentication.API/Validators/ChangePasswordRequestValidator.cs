using Authentication.API.DTOs.Auth.Requests;
using FluentValidation;

namespace Authentication.API.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password must not be empty");

        RuleFor(x => x.OldPasswordConfirm)
            .NotEmpty().WithMessage("'Old password confirm' must not be empty")
            .Equal(x => x.OldPassword).WithMessage("'Old Password' and 'Old Password Confirm' must match");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password must not be empty");
    }
}
