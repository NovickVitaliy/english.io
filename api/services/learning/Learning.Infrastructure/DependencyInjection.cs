using System.Reflection;
using Google.GenAI;
using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Providers;
using Learning.Application.Contracts.Repositories;
using Learning.Application.Contracts.Services;
using Learning.Infrastructure.Api;
using Learning.Infrastructure.Database;
using Learning.Infrastructure.Jobs;
using Learning.Infrastructure.Options;
using Learning.Infrastructure.Persistence;
using Learning.Infrastructure.Providers.DeckExporter;
using Learning.Infrastructure.Providers.DeckExporter.JsonExporter;
using Learning.Infrastructure.Providers.DeckImporter;
using Learning.Infrastructure.Repositories;
using Learning.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OfficeOpenXml;
using Polly;
using Polly.Registry;
using Polly.Retry;
using Quartz;
using Quartz.AspNetCore;
using QuestPDF.Infrastructure;
using Shared.MessageBus;
using Shared.Services;
using Shared.Services.Options;

namespace Learning.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

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


        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoOptions = sp.GetRequiredService<IOptions<MongoOptions>>().Value;

            return new MongoClient(mongoOptions.ConnectionString);
        });

        services.AddHttpClient();

        services.AddScoped<LearningDbContext>();
        services.AddScoped<IUserPreferencesRepository, UserPreferencesRepository>();
        services.AddScoped<IUserPreferencesService, UserPreferencesService>();

        services.AddScoped<IDecksRepository, DecksRepository>();
        services.AddScoped<IDecksService, DecksService>();

        services.AddScoped<IAiLearningService, GeminiAiLearningService>(sp =>
        {
            var geminiOptions = sp.GetRequiredService<IOptions<GeminiOptions>>();
            var aiLearningPromptsOptions = sp.GetRequiredService<IOptions<AiLearningPromptsOptions>>();
            var resiliencyPipelineProvider = sp.GetRequiredService<ResiliencePipelineProvider<string>>();

            var client = new Client(apiKey: geminiOptions.Value.ApiKey);

            return new GeminiAiLearningService(client, geminiOptions, aiLearningPromptsOptions, resiliencyPipelineProvider);
        });

        services.AddScoped<IDeckExporterService, DeckExporterService>();
        services.AddScoped<IDeckExporterFileProvider, CsvDeckExporterFileProvider>();
        services.AddScoped<IDeckExporterFileProvider, ExcelDeckExporterFileProvider>();
        services.AddScoped<IDeckExporterFileProvider, PdfDeckExporterFileProvider>();
        services.AddScoped<IDeckExporterFileProvider, JsonDeckExporterFileProvider>();

        services.AddScoped<IDeckImporterFileProvider, JsonDeckImporterFileProvider>();
        services.AddScoped<IDeckImporterService, DeckImporterService>();

        services.AddScoped<IPracticeRepository, PracticeRepository>();
        services.AddScoped<IPracticeService, PracticeService>();

        services.AddScoped<IWordUnitService, WordUnitService>();

        services.AddHttpContextAccessor();
        services.AddSharedServices();

        services.AddSignalR();

        services.AddStackExchangeRedisCache(options => { options.Configuration = configuration.GetConnectionString("RedisCache"); });

        services.AddQuartz(options =>
        {
            options.AddJob<NotificationJob>(c => c.StoreDurably().WithIdentity(NotificationJob.Name));

            options.UsePersistentStore(storeOptions =>
            {
                storeOptions.UsePostgres(cfg =>
                {
                    cfg.ConnectionString = configuration.GetConnectionString("SchedulingDatabase")!;
                    cfg.TablePrefix = "qrtz_";
                }, dataSourceName: "schedule-database");

                storeOptions.UseNewtonsoftJsonSerializer();
                storeOptions.UseProperties = true;
            });
        });

        services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });

        Setup.InitializeDatabase(configuration);

        services.AddOptions<NotificationsApiOptions>()
            .BindConfiguration(NotificationsApiOptions.ConfigurationKey)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddResiliencePipeline("gemini-api-pipeline", builder =>
        {
            builder.AddRetry(new RetryStrategyOptions()
            {
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(10),
                MaxRetryAttempts = 5,
                ShouldHandle = new PredicateBuilder()
                    .Handle<Exception>()
            });
        });

        services.AddHttpClient<IGrammarCheckerService, GrammarCheckerService>();

        return services;
    }
}
