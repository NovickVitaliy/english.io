using English.IO.Bot.Database;
using English.IO.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace English.IO.Bot.Providers.States.UserStates;

public class DbContextUserStateProvider : IUserStateProvider
{
    private readonly EnglishIOBotDbContext _dbContext;

    public DbContextUserStateProvider(EnglishIOBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserState> GetStateForUser(long userTelegramId)
    {
        var user = await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.TelegramChatId == userTelegramId);
        return user?.UserState ?? UserState.None;
    }

    public async Task SetStateForUser(long userTelegramId, UserState state)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TelegramChatId == userTelegramId);
        if (user is null)
        {
            return;
        }

        user.UserState = state;

        await _dbContext.SaveChangesAsync();
    }
}
