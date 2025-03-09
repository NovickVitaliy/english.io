using Learning.Features.Settings.Models;
using Microsoft.AspNetCore.Components;

namespace Learning.Features.Settings.Pages;

public partial class Settings : ComponentBase
{
    private Models.SettingsOptions CurrentOption { get; set; } = SettingsOptions.Security;
}

