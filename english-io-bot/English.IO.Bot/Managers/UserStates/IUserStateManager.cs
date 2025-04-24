using English.IO.Bot.Models;

namespace English.IO.Bot.Managers.UserStates;

public interface IUserStateManager
{
    Task<UserState> GetStateForUser(long userTelegramId);
    Task SetStateForUser(long userTelegramId, UserState state);
    Task RemoveStateForUser(long userTelegramId);
}