namespace Shared.Requests;

public record RequestValidationResult(bool IsValid, string ErrorMessage = "");