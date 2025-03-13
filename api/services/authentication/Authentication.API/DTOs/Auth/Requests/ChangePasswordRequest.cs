using Authentication.API.Validators;
using Shared.Requests;

namespace Authentication.API.DTOs.Auth.Requests;

public record ChangePasswordRequest(
    string OldPassword,
    string OldPasswordConfirm,
    string NewPassword) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new ChangePasswordRequestValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}
