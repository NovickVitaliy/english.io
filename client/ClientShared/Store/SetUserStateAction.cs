namespace Shared.Store;

public record SetUserStateAction(string AuthToken, string[] Role, string Email, string Username);