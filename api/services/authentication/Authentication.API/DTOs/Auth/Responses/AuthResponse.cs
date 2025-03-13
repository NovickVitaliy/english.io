namespace Authentication.API.DTOs.Auth.Responses;

public record AuthResponse(
    string Username,
    string Email,
    string[] Role,
    string AuthToken,
    bool IsEmailVerified);
