using Authentication.Features.ForgotPassword.Components;
using Shared.LocalizedDataAnnotations;

namespace Authentication.Features.ForgotPassword.Models;

public class ForgotPasswordRequest
{
    public required Uri ResetPasswordUrl { get; init; }

    [LocalizedRequired(Constants.Localization.ForgotPasswordFormBaseName, Constants.ValidationErrorsName.Shared.EmailIsRequired, typeof(ForgotPasswordForm))]
    [LocalizedEmailAddress(Constants.Localization.ForgotPasswordFormBaseName, Constants.ValidationErrorsName.Shared.EmailMustBeInValidFormat, typeof(ForgotPasswordForm))]
    public string Email { get; set; } = null!;
}
