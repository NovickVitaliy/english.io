using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Learning.Features.Home.Components;

public partial class QuickAccess : ComponentBase
{
    [Inject] private IStringLocalizer<QuickAccess> Localizer { get; init; } = null!;
}

