using Authentication.Features.ResetPassword.Components;
using Shared.LocalizedDataAnnotations;

namespace Authentication.Features.ResetPassword.Models;

public class ResetPasswordRequest
{
    [LocalizedRequired(Constants.Localization.ResetPasswordFormBaseName, Constants.ValidationErrorsName.Shared.PasswordIsRequired, typeof(ResetPasswordForm))]
    public string NewPassword { get; set; } = null!;

    [LocalizedRequired(Constants.Localization.ResetPasswordFormBaseName, Constants.ValidationErrorsName.Shared.ConfirmPasswordIsRequired, typeof(ResetPasswordForm))]
    [LocalizedCompare(Constants.Localization.ResetPasswordFormBaseName, Constants.ValidationErrorsName.Shared.PasswordAndConfirmPasswordMustBeEqual, typeof(ResetPasswordForm), nameof(NewPassword))]
    public string NewPasswordConfirm { get; set; } = null!;

    public string ResetToken { get; set; } = null!;
}
