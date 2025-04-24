using English.IO.Bot.Models;

namespace English.IO.Bot.Providers.States.UserStates;

public interface IUserStateProvider
{
    Task<UserState> GetStateForUser(long userTelegramId);
    Task SetStateForUser(long userTelegramId, UserState state);
}