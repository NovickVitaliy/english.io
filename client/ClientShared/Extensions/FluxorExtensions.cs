using System.Reflection;
using Blazored.LocalStorage;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.Extensions.DependencyInjection;
using Shared.Store;

namespace Shared.Extensions;

public static class FluxorExtensions
{
    public static IServiceCollection ConfigureStateManager(this IServiceCollection services, Assembly[]? assemblies = null)
    {
        services.AddBlazoredLocalStorage();
        services.AddFluxor(options =>
        {
            options.ScanAssemblies(typeof(FluxorExtensions).Assembly, assemblies);
            options.UseReduxDevTools();
            options.AddMiddleware<ApiLoadingMiddleware>();
        });

        return services;
    }
}
