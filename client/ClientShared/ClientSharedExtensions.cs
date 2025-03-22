using Microsoft.Extensions.DependencyInjection;
using Shared.DelegateHttpHandlers;
using Shared.Options;

namespace Shared;

public static class ClientSharedExtensions
{
    public static IServiceCollection RegisterClientSharedDependencies(this IServiceCollection services)
    {
        services.AddTransient<LocalizationHttpHandler>();

        services.AddOptions<ClientOptions>()
            .BindConfiguration(ClientOptions.ClientOptionsKey)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
