using Fluxor;
using Microsoft.AspNetCore.Components;
using Shared.Store.Theme;

namespace Learning.Features.DashboardLayout.Components;

public partial class DashboardHeader : ComponentBase
{
    [Parameter] public EventCallback<bool> OnThemeChange { get; set; }
    [Parameter] public bool IsDarkMode { get; set; }
    [Inject] private IDispatcher Dispatcher { get; set; } = null!;

    private async Task ChangeTheme(bool value)
    {
        await OnThemeChange.InvokeAsync(value);
        Dispatcher.Dispatch(new SetThemeStateAction(value));
    }
}

