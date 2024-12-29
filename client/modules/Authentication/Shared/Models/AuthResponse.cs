namespace Authentication.Shared.Models;

public record AuthResponse(
    string Username,
    string Email,
    string[] Role,
    string AuthToken);