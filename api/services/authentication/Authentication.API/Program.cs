using System.Reflection;
using Authentication.API;
using Authentication.API.Data;
using Authentication.API.Data.Seed;
using Authentication.API.Models;
using Authentication.API.Options;
using Authentication.API.Services.AuthService;
using Authentication.API.Services.TokenGenerator;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Shared.Authentication;
using Shared.MessageBus;
using Shared.Services;
using Shared.Services.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddOptions<NotificationsApiOptions>()
    .BindConfiguration(NotificationsApiOptions.ConfigurationKey)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<ForgotPasswordOptions>()
    .BindConfiguration(ForgotPasswordOptions.ConfigurationKey)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<EmailVerificationOptions>()
    .BindConfiguration(EmailVerificationOptions.ConfigurationKey)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpContextAccessor();
builder.Services.AddNotificationsServiceHttpClient();
builder.Services.ConfigureJwtAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureRabbitMq(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(AuthDbContext.DefaultConnectionStringPosition));
});

builder.Services.AddProblemDetails();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();
    await app.SeedRolesAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
