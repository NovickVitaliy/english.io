namespace Shared.ErrorHandling;

public static class ErrorMessages
{
    public const string NoError = "No error";
    public static string BadRequest(string reason) => $"{reason}";
    public static string Unauthorized(string reason) => $"{reason}";
    public static string Forbidden(string reason) => $"{reason}";
    public static string NotFound(object key) => $"Entity_Not_Found";
    public static string Conflict(string reason) => $"{reason}";
    public static string InternalServerError(string reason) => $"{reason}";
}
