using System.Reflection;
using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Providers;
using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Infrastructure.Api;
using Learning.Infrastructure.Database;
using Learning.Infrastructure.Options;
using Learning.Infrastructure.Providers.DeckExporter;
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

        services.AddOptions<AiLearningPromptsOptions>()
            .BindConfiguration(AiLearningPromptsOptions.ConfigurationKey)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<GeminiOptions>()
            .BindConfiguration(GeminiOptions.ConfigurationKey)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient(IAiLearningService.HttpClientKey, (sp, client) =>
        {
            var geminiOptions = sp.GetRequiredService<IOptions<GeminiOptions>>().Value ?? throw new InvalidOperationException();

            client.BaseAddress = new Uri(geminiOptions.GenerateContentUrl);
        });

        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoOptions = sp.GetRequiredService<IOptions<MongoOptions>>().Value;

            return new MongoClient(mongoOptions.ConnectionString);
        });

        services.AddHttpContextAccessor();
        services.AddHttpClient();

        services.AddScoped<LearningDbContext>();
        services.AddScoped<IUserPreferencesRepository, UserPreferencesRepository>();
        services.AddScoped<IUserPreferencesService, UserPreferencesService>();

        services.AddScoped<IDecksRepository, DecksRepository>();
        services.AddScoped<IDecksService, DecksService>();

        services.AddScoped<IAiLearningService, GeminiAiLearningService>();
        services.AddScoped<IDeckExporterService, DeckExporterService>();
        services.AddScoped<IDeckExporterFileProvider, CsvDeckExporterFileProvider>();
        services.AddScoped<IDeckExporterFileProvider, ExcelDeckExporterFileProvider>();

        return services;
    }
}
