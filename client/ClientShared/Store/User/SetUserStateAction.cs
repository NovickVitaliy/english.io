namespace Shared.Store.User;

public record SetUserStateAction(string AuthToken, string[] Role, string Email, string Username, bool IsEmailVerified);
