using Fluxor;
using MudBlazor;

namespace Shared.Store.Loading;

public class LoadingStateEffects
{
    private readonly ISnackbar _snackbar;

    public LoadingStateEffects(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    [EffectMethod]
    public Task HandleApiRequestStartedAction(ApiRequestStartedAction action, IDispatcher dispatcher)
    {
        _snackbar.Add("Started processing", Severity.Info);
        return Task.CompletedTask;
    }
}
