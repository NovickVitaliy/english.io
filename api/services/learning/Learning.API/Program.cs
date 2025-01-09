using Learning.Infrastructure;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
