namespace Shared.Store;

public class SetUserStateAction
{
    public string Token { get; }

    public SetUserStateAction(string token)
    {
        Token = token;
    }
}