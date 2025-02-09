namespace Shared.ErrorHandling;

public static class ErrorMessages
{
    public const string NoError = "No error";
    public static string BadRequest(string reason) => $"Bad Request: {reason}";
    public static string Unauthorized(string reason) => $"Unauthorized: {reason}";
    public static string Forbidden(string reason) => $"Forbidden: {reason}";
    public static string NotFound<T>(object key) => $"{typeof(T).Name} with key '{key}' was not found.";
    public static string Conflict(string reason) => $"Conflict: {reason}";
    public static string InternalServerError(string reason) => $"Internal Server Error: {reason}";
}
