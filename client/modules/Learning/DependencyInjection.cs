using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Learning;

public static class DependencyInjection
{
    public static IServiceCollection RegisterLearningModule(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(LearningConstants.AuthorizationPolicies.PreferencesMustNotBeConfigured,
                builder => builder.RequireClaim(GlobalConstants.ApplicationClaimTypes.PreferencesConfigured, "false"));
        
        return services;
    }
}