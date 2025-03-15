using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Learning.Features.Settings.Components.SecurityComponent;

public partial class Security : ComponentBase
{
    [Inject]
    private IStringLocalizer<Security> Localizer { get; init; } = null!;
}

