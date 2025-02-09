using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Shared.Extensions;

public static class RefitExtensions
{
    private const string ServicesSectionKey = "Services";

    public static IServiceCollection ConfigureApiService<TServiceType>(
        this IServiceCollection services,
        IConfiguration configuration,
        string apiServiceUrlKey) where TServiceType : class
    {
        services.AddRefitClient<TServiceType>()
            .ConfigureHttpClient(options =>
            {
                options.BaseAddress = new Uri(configuration.GetSection(ServicesSectionKey)[apiServiceUrlKey]
                                              ?? throw new ArgumentNullException(
                                                  $"Url for service with key: '{apiServiceUrlKey}' was not found"));
            });

        return services;
    }

    public static ProblemDetails ToProblemDetails(this ApiException apiException)
    {
        return JsonSerializer.Deserialize<ProblemDetails>(apiException.Content!, ClientConstants.JsonSerializerOptions)!;
    }
}
