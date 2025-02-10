namespace Authentication.API.DTOs.Auth.Requests;

public record ForgotPasswordRequest(string ResetPasswordUrl, string Email);
