using System.Net;
using Client;
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

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization(DependencyInjection.GetLocalizationOptions(app.Configuration));

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();