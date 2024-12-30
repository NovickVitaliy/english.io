using System.ComponentModel.DataAnnotations;
using Authentication.Features.Login.Components;
using Authentication.Features.Register.Components;
using Shared.LocalizedDataAnnotations;

namespace Authentication.Features.Login.Models;

public class LoginRequest
{
    [LocalizedEmailAddress(Constants.Localization.LoginFormBaseName, Constants.ValidationErrorsName.Login.EmailMustBeInValidFormat, typeof(LoginForm))]
    [LocalizedRequired(Constants.Localization.LoginFormBaseName, Constants.ValidationErrorsName.Login.EmailIsRequired, typeof(LoginForm))]
    public string Email { get; set; } = null!;

    [LocalizedRequired(Constants.Localization.LoginFormBaseName, Constants.ValidationErrorsName.Login.PasswordIsRequired, typeof(RegisterForm))]
    public string Password { get; set; } = null!;
}