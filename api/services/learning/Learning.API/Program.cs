using Learning;
using Learning.Application;
using Learning.Infrastructure;
using Learning.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .ConfigureInfrastructure(builder.Configuration)
    .ConfigureApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ConfigureDatabase();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
