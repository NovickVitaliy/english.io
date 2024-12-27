namespace Authentication.API.DTOs.Auth.Requests;

public record LoginUserRequest(
    string Email,
    string Password);