using Authentication.API.Validators;
using Shared.Requests;

namespace Authentication.API.DTOs.Other;

public record ResetPasswordRequest(
    string NewPassword,
    string NewPasswordConfirm,
    string Email,
    string ResetToken) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new ResetPasswordRequestValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}
