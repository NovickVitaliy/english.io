using Learning;
using Learning.Application;
using Learning.Infrastructure;
using Shared.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .ConfigureInfrastructure(builder.Configuration)
    .ConfigureApplication()
    .ConfigureJwtAuthentication();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ConfigureDatabase();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
