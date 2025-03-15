using Fluxor;
using Shared.Store.User.Actions;

namespace Shared.Store.User;

public static class UserReducers
{
    [ReducerMethod]
    public static UserState SetUserState(UserState state, SetUserStateAction action) => new(action.AuthToken, action.Role, action.Email, action.Username, action.IsEmailVerified);

    [ReducerMethod]
    public static UserState VerifyUserEmail(UserState state, VerifyUserEmailAction action) => state with
    {
        IsEmailVerified = true
    };

    [ReducerMethod]
    public static UserState SetNewAccessToken(UserState state, SetNewAccessTokenAction action) => state with
    {
        Token = action.NewAccessToken
    };
}
