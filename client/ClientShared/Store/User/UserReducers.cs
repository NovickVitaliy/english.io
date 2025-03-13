using Fluxor;

namespace Shared.Store.User;

public static class UserReducers
{
    [ReducerMethod]
    public static UserState SetUserState(UserState state, SetUserStateAction action) => new(action.AuthToken, action.Role, action.Email, action.Username, action.IsEmailVerified);
}
