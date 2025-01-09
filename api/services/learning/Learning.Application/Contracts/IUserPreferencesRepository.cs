using Learning.Domain;
using Shared.ErrorHandling;

namespace Learning.Application.Contracts;

public interface IUserPreferencesRepository
{
    Task<Result<UserPreferences>> GetUserPreferencesAsync(string email);
    Task<Result<Guid>> CreateUserPreferencesAsync(UserPreferences userPreferences);
    Task<Result<bool>> UpdateUserPreferencesAsync(Guid id, UserPreferences userPreferences);
    Task<Result<bool>> DeleteUserPreferencesAsync(Guid id);
}