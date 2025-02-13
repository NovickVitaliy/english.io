namespace Notifications.API.Services.ApiKey;

public interface IApiKeyService
{
    Task<bool> IsApiKeyValid(string apiKey);
}
