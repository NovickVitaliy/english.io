using Learning.Features.PreferenceConfiguring.Models;
using Refit;

namespace Learning.Features.PreferenceConfiguring.Services;

public interface IUserPreferencesService
{
    const string ApiUrlKey = "UserPreferences";
    
    [Post("/user-preferences")]
    Task<Guid> CreateUserPreferencesAsync(CreateUserPreferencesRequest request);
}