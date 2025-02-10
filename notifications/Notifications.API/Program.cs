using System.Text.Json.Serialization;
using FluentValidation;
using Notifications.API.Options;
using Notifications.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOptions<MailSettings>()
    .BindConfiguration(MailSettings.ConfigurationKey)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddScoped<IEmailService, MailKitEmailService>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

app.MapControllers();

await app.RunAsync();
