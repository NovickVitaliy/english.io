namespace Shared.Store.User.Actions;

public record SetUserStateAction(string AuthToken, string[] Role, string Email, string Username, bool IsEmailVerified);
