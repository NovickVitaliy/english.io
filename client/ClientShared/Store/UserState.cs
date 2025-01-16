using Fluxor;

namespace Shared.Store;

[FeatureState]
public class UserState
{
    public string Token { get; }

    private UserState()
    {
        
    }

    public UserState(string token)
    {
        Token = token;
    }
}