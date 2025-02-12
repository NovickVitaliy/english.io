using Authentication.API.Models;

namespace Authentication.API.DTOs.Auth.Requests;

public record RegisterUserRequest(
        string Email,
        string Password,
        string ConfirmPassword
    ) : IAuthenticationRequest;
