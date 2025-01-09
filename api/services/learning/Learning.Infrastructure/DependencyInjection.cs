using Learning.Application.Contracts;
using Learning.Application.Contracts.Repositories;
using Learning.Domain;
using Learning.Infrastructure.Database;
using Learning.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Learning.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MongoOptions>()
            .BindConfiguration(MongoOptions.ConfigurationKey)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoOptions = sp.GetRequiredService<IOptions<MongoOptions>>().Value;

            return new MongoClient(mongoOptions.ConnectionString);
        });

        services.AddScoped<LearningDbContext>();
        services.AddScoped<IUserPreferencesRepository, IUserPreferencesRepository>();
        
        return services;
    }
}