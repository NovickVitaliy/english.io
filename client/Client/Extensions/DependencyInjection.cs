using System.Net;
using Authentication;
using MudBlazor.Services;

namespace Client.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterAuthenticationModule(configuration);
        
        return services; 
    }

    public static IServiceCollection ConfigureBlazor(this IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddMudServices();

        return services;
    }

    public static WebApplicationBuilder ConfigureBlazorCore(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Loopback, 4998);
            options.Listen(IPAddress.Parse("192.168.1.225"), 4998);
        });

        return builder;
    }
}