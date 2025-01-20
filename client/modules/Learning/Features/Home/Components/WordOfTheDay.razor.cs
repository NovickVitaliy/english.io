using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Learning.Features.Home.Components;

public partial class WordOfTheDay : ComponentBase
{
    [Inject] private IStringLocalizer<WordOfTheDay> Localizer { get; init; } = null!;
}

