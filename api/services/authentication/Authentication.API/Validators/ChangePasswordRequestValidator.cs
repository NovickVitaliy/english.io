using Authentication.API.DTOs.Auth.Requests;
using FluentValidation;
using static Authentication.API.LocalizationKeys;

namespace Authentication.API.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage(OldPasswordMustNotBeEmpty);

        RuleFor(x => x.OldPasswordConfirm)
            .NotEmpty().WithMessage(OldPasswordConfirmMustNotBeEmpty)
            .Equal(x => x.OldPassword).WithMessage(OldPasswordAndConfirmMustMatch);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(NewPasswordMustNotBeEmpty);
    }
}
