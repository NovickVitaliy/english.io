using Learning.Domain.Models;

namespace Learning.Application.Contracts.Repositories;

public interface IUserPreferencesRepository
{
    Task<UserPreferences?> GetUserPreferencesAsync(Guid id);
    Task<Guid> CreateUserPreferencesAsync(UserPreferences userPreferences);
    Task<bool> UpdateUserPreferencesAsync(Guid id, UserPreferences userPreferences);
    Task<bool> DeleteUserPreferencesAsync(Guid id);
}
