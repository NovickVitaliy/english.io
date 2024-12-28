namespace Authentication.Shared.Models;

public class AuthResponse(
    string Username,
    string Email,
    string[] Role,
    string AuthToken);