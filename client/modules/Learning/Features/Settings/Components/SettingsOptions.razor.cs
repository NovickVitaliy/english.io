using Learning.Features.Settings.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Learning.Features.Settings.Components;

public partial class SettingsOptions : ComponentBase
{
    [Inject]
    private IStringLocalizer<SettingsOptions> Localizer { get; init; } = null!;

    [Parameter, EditorRequired]
    public EventCallback<Models.SettingsOptions> OnOptionChanged { get; init; }
}

