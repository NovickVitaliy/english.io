using Learning.Application;
using Learning.Infrastructure;
using Learning.Infrastructure.Database;
using Learning.Infrastructure.Hubs;
using Shared.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();
builder.Services
    .ConfigureInfrastructure(builder.Configuration)
    .ConfigureApplication()
    .ConfigureJwtAuthentication();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.ConfigureDatabase();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ConnectingTelegramNotificationChannelHub>("/connect-telegram-notification-channel-hub");

await app.RunAsync();
