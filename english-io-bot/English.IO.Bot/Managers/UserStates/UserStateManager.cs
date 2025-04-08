using English.IO.Bot.Models;
using English.IO.Bot.Providers.States.UserStates;

namespace English.IO.Bot.Managers.UserStates;

public class UserStateManager : IUserStateManager
{
    private readonly IUserStateProvider _userStateProvider;
    
    public UserStateManager(IUserStateProvider userStateProvider)
    {
        _userStateProvider = userStateProvider;
    }
    
    public Task<UserState> GetStateForUser(long userTelegramId)
    {
        return _userStateProvider.GetStateForUser(userTelegramId);
    }
    
    public Task SetStateForUser(long userTelegramId, UserState state)
    {
        return _userStateProvider.SetStateForUser(userTelegramId, state);
    }
    
    public Task RemoveStateForUser(long userTelegramId)
    {
        return _userStateProvider.SetStateForUser(userTelegramId, UserState.None);
    }
}