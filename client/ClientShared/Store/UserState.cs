using Fluxor;

namespace Shared.Store;

[FeatureState]
public record UserState(string Token, string[] Role, string Email, string Username)
{
    private UserState() : this(null!, null!, null!, null!)
    {
        
    }
}