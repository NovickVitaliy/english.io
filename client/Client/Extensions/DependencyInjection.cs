using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Authentication;
using Learning;
using Learning.Store.Decks;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using Shared.Extensions;

namespace Client.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterAuthenticationModule(configuration);
        services.RegisterLearningModule(configuration);
        return services;
    }

    public static IServiceCollection ConfigureBlazor(this IServiceCollection services)
    {
        services.ConfigureStateManager([
            typeof(DecksState).Assembly
        ]);
        services.AddRazorPages();
        services.AddControllers()
            .AddApplicationPart(typeof(IAuthenticationMarker).Assembly)
            .AddApplicationPart(typeof(ILearningMarker).Assembly);
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddMudServices();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/";
            });
        services.AddAuthorization();
        services.AddCascadingAuthenticationState();
        return services;
    }

    public static RequestLocalizationOptions GetLocalizationOptions(IConfiguration configuration)
    {
        var cultures = configuration.GetSection("Cultures")
            .GetChildren().ToDictionary(x => x.Key, x => x.Value);

        var supportedCultures = cultures.Keys.ToArray();

        var requestLocalizationOptions = new RequestLocalizationOptions()
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        return requestLocalizationOptions;
    }

    public static WebApplicationBuilder ConfigureBlazorCore(this WebApplicationBuilder builder)
    {
        // builder.WebHost.ConfigureKestrel(options =>
        // {
        //     var ipv4Address = GetIpv4();
        //     options.Listen(IPAddress.Loopback, 4998);
        //     options.Listen(IPAddress.Parse(ipv4Address), 4998);
        // });

        return builder;
    }

    // /// <summary>
    // /// for development purposes only
    // /// </summary>
    // /// <returns></returns>
    // private static string GetIpv4()
    // {
    //     return Dns.GetHostEntry(Dns.GetHostName())
    //         .AddressList
    //         .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
    //         ?.ToString() ?? throw new NetworkInformationException();
    // }
}
