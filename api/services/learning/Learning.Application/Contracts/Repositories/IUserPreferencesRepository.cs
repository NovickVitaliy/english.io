using Learning.Domain;
using Shared.ErrorHandling;

namespace Learning.Application.Contracts.Repositories;

public interface IUserPreferencesRepository
{
    Task<UserPreferences?> GetUserPreferencesAsync(string email);
    Task<Guid> CreateUserPreferencesAsync(UserPreferences userPreferences);
    Task<bool> UpdateUserPreferencesAsync(Guid id, UserPreferences userPreferences);
    Task<bool> DeleteUserPreferencesAsync(Guid id);
}