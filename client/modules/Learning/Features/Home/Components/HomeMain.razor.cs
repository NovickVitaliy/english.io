using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Features.Home.Components;

public partial class HomeMain : FluxorComponent
{
    [Inject] private IStringLocalizer<HomeMain> Localizer { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
}

