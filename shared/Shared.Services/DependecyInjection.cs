using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Shared.Services.Contracts;
using Shared.Services.Options;
using Shared.Services.Services;

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

    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        // make sure to add http context accessor in the project where ICurrentUserAccessor is going to be used
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        services.AddNotificationsServiceHttpClient();

        return services;
    }
}
