namespace Shared.Models;

public record UserData(
    string Username,
    string Email,
    string[] Role,
    string AuthToken,
    bool IsEmailVerified);
