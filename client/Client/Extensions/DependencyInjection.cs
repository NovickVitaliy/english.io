using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
            var ipv4Address = GetIpv4();
            options.Listen(IPAddress.Loopback, 4998);
            options.Listen(IPAddress.Parse(ipv4Address), 4998);
        });

        return builder;
    }

    /// <summary>
    /// for development purposes only
    /// </summary>
    /// <returns></returns>
    private static string GetIpv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList
            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
            ?.ToString() ?? throw new NetworkInformationException();
    }
}