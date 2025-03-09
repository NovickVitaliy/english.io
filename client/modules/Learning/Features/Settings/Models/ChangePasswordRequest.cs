using Learning.Features.Settings.Components.SecurityComponent;
using Shared.LocalizedDataAnnotations;

namespace Learning.Features.Settings.Models;

public class ChangePasswordRequest
{
    [LocalizedRequired(Constants.Localization.ChangePasswordComponentBaseName, Constants.ValidationErrors.ChangePasswordComponent.OldPasswordIsRequired, typeof(ChangePasswordComponent))]
    public string OldPassword { get; set; } = null!;

    [LocalizedRequired(Constants.Localization.ChangePasswordComponentBaseName, Constants.ValidationErrors.ChangePasswordComponent.NewPasswordIsRequired, typeof(ChangePasswordComponent))]
    public string NewPassword { get; set; } = null!;

    [LocalizedRequired(Constants.Localization.ChangePasswordComponentBaseName, Constants.ValidationErrors.ChangePasswordComponent.NewPasswordConfirmIsRequired, typeof(ChangePasswordComponent))]
    [LocalizedCompare(Constants.Localization.ChangePasswordComponentBaseName, Constants.ValidationErrors.ChangePasswordComponent.NewPasswordsMustMatch, typeof(ChangePasswordComponent), nameof(NewPasswordConfirm))]
    public string NewPasswordConfirm { get; set; } = null!;
}
