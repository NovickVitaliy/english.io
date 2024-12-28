using System.Net;
using Client.Components;
using Client.Extensions;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBlazorCore();
builder.Services.ConfigureBlazor();
builder.Services.ConfigureModules(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();