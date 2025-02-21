using Learning.Application.DTOs.UserPreferences;
using Shared.ErrorHandling;

namespace Learning.Application.Contracts.Services;

public interface IUserPreferencesService
{
    Task<Result<string>> CreateUserPreferencesAsync(CreateUserPreferencesRequest request);
    Task<Result<UserPreferencesDto>> GetUserPreferencesAsync(GetUserPreferencesRequest request);
    Task<Result<bool>> UpdateUserPreferencesAsync(UpdateUserPreferencesRequest request);
    Task<Result<bool>> DeleteUserPreferencesAsync(DeleteUserPreferencesRequest request);
}
