using System.Reflection;
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
using Learning.Infrastructure.Repositories;
using Learning.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OfficeOpenXml;
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
        services.AddScoped<IDeckExporterFileProvider, PdfDeckExporterFileProvider>();

        services.AddScoped<IPracticeRepository, PracticeRepository>();
        services.AddScoped<IPracticeService, PracticeService>();

        services.AddHttpContextAccessor();
        services.AddSharedServices();

        services.AddSignalR();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisCache");
        });

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

        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        Setup.InitializeDatabase(configuration);

        services.AddOptions<NotificationsApiOptions>()
            .BindConfiguration(NotificationsApiOptions.ConfigurationKey)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
