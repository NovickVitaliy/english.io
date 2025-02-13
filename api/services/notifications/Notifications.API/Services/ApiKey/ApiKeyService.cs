using Microsoft.EntityFrameworkCore;
using Notifications.API.Database;

namespace Notifications.API.Services.ApiKey;

public class ApiKeyService : IApiKeyService
{
    private readonly NotificationsDbContext _dbContext;

    public ApiKeyService(NotificationsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsApiKeyValid(string apiKey)
    {
        return await _dbContext.ApiKeys.SingleOrDefaultAsync(x => x.Key == apiKey) != null;
    }
}
