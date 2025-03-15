using Authentication.API.DTOs.Other;
using FluentValidation;
using static Authentication.API.LocalizationKeys;

namespace Authentication.API.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(EmailCannotBeEmpty)
            .EmailAddress().WithMessage(EmailMustBeInValidFormat);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(NewPasswordMustNotBeEmpty);

        RuleFor(x => x.NewPasswordConfirm)
            .NotEmpty().WithMessage(NewConfirmPasswordCannotBeEmpty)
            .Equal(x => x.NewPassword).WithMessage(NewPasswordAndNewConfirmPasswordMustMatch);

        RuleFor(x => x.ResetToken)
            .NotEmpty().WithMessage(ResetTokenCannotBeEmpty);
    }
}
