using Learning.Features.PreferenceConfiguring.Models;
using Refit;

namespace Learning.Features.PreferenceConfiguring.Services;

public interface IUserPreferencesService
{
    const string ApiUrlKey = "Learning";

    [Post("/user-preferences")]
    Task<string> CreateUserPreferencesAsync(CreateUserPreferencesRequest request, [Authorize] string token);
}
