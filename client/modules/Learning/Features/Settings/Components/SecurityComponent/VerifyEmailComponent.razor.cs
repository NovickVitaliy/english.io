using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Shared.Store.User;

namespace Learning.Features.Settings.Components.SecurityComponent;

public partial class VerifyEmailComponent : FluxorComponent
{
    [Inject]
    private IStringLocalizer<VerifyEmailComponent> Localizer { get; init; } = null!;

    [Inject]
    private IState<UserState> UserState { get; init; } = null!;


    private Task VerifyEmailAsync()
    {
        throw new NotImplementedException();
    }
}

