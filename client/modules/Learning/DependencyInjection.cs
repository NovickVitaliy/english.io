using Learning.Features.DashboardLayout.Components;
using Learning.Features.GrammarChecker.Services;
using Learning.Features.Home.Services;
using Learning.Features.Practice.Services;
using Learning.Features.PreferenceConfiguring.Options;
using Learning.Features.PreferenceConfiguring.Services;
using Learning.Features.Settings.Service;
using Learning.LearningShared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Shared.Extensions;

namespace Learning;

public static class DependencyInjection
{
    public static IServiceCollection RegisterLearningModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(LearningConstants.AuthorizationPolicies.PreferencesMustNotBeConfigured,
                builder => builder.RequireClaim(GlobalConstants.ApplicationClaimTypes.PreferencesConfigured, "false"));

        services.ConfigureApiService<IUserPreferencesService>(configuration, IUserPreferencesService.ApiUrlKey);
        services.ConfigureApiService<IDecksService>(configuration, IDecksService.ApiUrlKey);
        services.ConfigureApiService<IAuthenticationSettingsService>(configuration, IAuthenticationSettingsService.ApiUrlKey);
        services.ConfigureApiService<ITextToSpeechService>(configuration, ITextToSpeechService.ApiUrlKey);
        services.ConfigureApiService<IPracticeService>(configuration, IPracticeService.ApiUrlKey);
        services.ConfigureApiService<IStatisticsApi>(configuration, IStatisticsApi.ApiUrlKey);

        services.AddHttpClient<AiChatService>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetSection("Services")[IUserPreferencesService.ApiUrlKey]
                                         ?? throw new NotImplementedException());
        });

        services.AddHttpClient<GrammarCheckerService>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetSection("Services")[IUserPreferencesService.ApiUrlKey]
                                         ?? throw new NotImplementedException());
        });

        services.AddOptions<PreferencesConfiguringHubOptions>()
            .BindConfiguration(PreferencesConfiguringHubOptions.Key)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
