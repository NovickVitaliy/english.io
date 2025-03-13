using Fluxor;

namespace Shared.Store.User;

[FeatureState]
public record UserState(string Token, string[] Role, string Email, string Username, bool IsEmailVerified)
{
    private UserState() : this(null!, null!, null!, null!, false)
    {

    }
}
