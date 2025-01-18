using Authentication.Features.Register.Components;
using Shared.LocalizedDataAnnotations;

namespace Authentication.Features.Register.Models;

public class RegisterRequest
{
    [LocalizedRequired(Constants.Localization.RegisterFormBaseName, Constants.ValidationErrorsName.Register.EmailIsRequired, typeof(RegisterForm))]
    [LocalizedEmailAddress(Constants.Localization.RegisterFormBaseName, Constants.ValidationErrorsName.Register.EmailMustBeInValidFormat, typeof(RegisterForm))]
    public string Email { get; set; } = null!;

    [LocalizedRequired(Constants.Localization.RegisterFormBaseName, Constants.ValidationErrorsName.Register.PasswordIsRequired, typeof(RegisterForm))]
    public string Password { get; set; } = null!;

    [LocalizedRequired(Constants.Localization.RegisterFormBaseName, Constants.ValidationErrorsName.Register.ConfirmPasswordIsRequired, typeof(RegisterForm))]
    [LocalizedCompare(Constants.Localization.RegisterFormBaseName, Constants.ValidationErrorsName.Register.PasswordAndConfirmPasswordMustBeEqual, typeof(RegisterForm),
        nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
}