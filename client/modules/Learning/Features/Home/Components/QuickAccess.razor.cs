using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Learning.Features.Home.Components;

public partial class QuickAccess : Fluxor.Blazor.Web.Components.FluxorComponent
{
    [Inject] private IStringLocalizer<QuickAccess> Localizer { get; init; } = null!;
}

