using Shared.Requests;

namespace Authentication.API.DTOs.Auth.Requests;

public record ChangePasswordRequest(
    string OldPassword,
    string OldPasswordConfirm,
    string NewPassword) : IBaseRequest
{
    public RequestValidationResult IsValid() => throw new NotImplementedException();
}
