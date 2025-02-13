using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Authentication;
using Notifications.API.Database;
using Notifications.API.Extensions;
using Notifications.API.Options;
using Notifications.API.Services;
using Notifications.API.Services.ApiKey;
using Notifications.API.Services.Email;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("ApiKey")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);

builder.Services.AddDbContext<NotificationsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")
                      ?? throw new InvalidOperationException("Connection string was not found in the configuration")));

builder.Services.AddScoped<IApiKeyService, ApiKeyService>();

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

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
