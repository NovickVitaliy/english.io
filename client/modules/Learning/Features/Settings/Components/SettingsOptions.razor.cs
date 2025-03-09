using Learning.Features.Settings.Models;
using Microsoft.AspNetCore.Components;

namespace Learning.Features.Settings.Components;

public partial class SettingsOptions : ComponentBase
{
    [Parameter, EditorRequired]
    public EventCallback<Models.SettingsOptions> OnOptionChanged { get; init; }
}

