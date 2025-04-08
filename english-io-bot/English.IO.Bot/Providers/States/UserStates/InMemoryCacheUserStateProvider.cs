using English.IO.Bot.Models;
using Microsoft.Extensions.Caching.Memory;

namespace English.IO.Bot.Providers.States.UserStates;

public class InMemoryCacheUserStateProvider : IUserStateProvider
{
    private readonly IMemoryCache _cache;
    public InMemoryCacheUserStateProvider(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<UserState> GetStateForUser(long userTelegramId)
    {
        var storedState = _cache.Get(userTelegramId)?.ToString();
        return Task.FromResult(storedState is null 
            ? UserState.None 
            : Enum.Parse<UserState>(storedState, true));
    }

    public Task SetStateForUser(long userTelegramId, UserState state)
    {
        _cache.Set(userTelegramId, state.ToString(), TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }
}