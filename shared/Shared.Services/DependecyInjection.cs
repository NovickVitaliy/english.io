using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Shared.Services.Options;

namespace Shared.Services;

public static class DependecyInjection
{
    public static IServiceCollection AddNotificationsServiceHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(SharedServicesConstants.NotificationHttpClientName, (sp,client) =>
        {
            var notiticationApiOptions = sp.GetRequiredService<IOptions<NotificationsApiOptions>>().Value;
            client.BaseAddress = new Uri(notiticationApiOptions.IsHttps ? notiticationApiOptions.Https : notiticationApiOptions.Http);
            client.DefaultRequestHeaders.Add("X-API-KEY", notiticationApiOptions.Key);
        }).AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(3));

        return services;
    }
}
