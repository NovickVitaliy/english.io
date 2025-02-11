namespace Authentication.API.DTOs.Other;

public record ResetPasswordRequest(
    string NewPassword,
    string NewPasswordConfirm,
    string Email,
    string ResetToken);
