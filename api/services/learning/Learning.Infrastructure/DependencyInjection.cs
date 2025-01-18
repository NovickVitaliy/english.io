using System.Reflection;
using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Infrastructure.Database;
using Learning.Infrastructure.Options;
using Learning.Infrastructure.Repositories;
using Learning.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.MessageBus;

namespace Learning.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureRabbitMq(Assembly.GetExecutingAssembly());
        
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
        services.AddScoped<IUserPreferencesRepository, UserPreferencesRepository>();
        services.AddScoped<IUserPreferencesService, UserPreferencesService>();
        
        return services;
    }
}