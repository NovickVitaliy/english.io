using Authentication.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions;

namespace Authentication;

public static class DependencyInjection
{
    public static IServiceCollection RegisterAuthenticationModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureApiService<IAuthenticationService>(configuration, IAuthenticationService.ApiUrlKey);

        return services;
    }
}
