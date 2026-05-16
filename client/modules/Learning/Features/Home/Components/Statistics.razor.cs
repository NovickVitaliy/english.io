using Fluxor;
using Learning.Store.Statistics;
using Microsoft.AspNetCore.Components;
using Shared.Store.User;

namespace Learning.Features.Home.Components;

public partial class Statistics : Fluxor.Blazor.Web.Components.FluxorComponent
{
    [Inject] private IDispatcher Dispatcher { get; set; } = null!;
    [Inject] private IState<StatisticsState> StatisticsState { get; set; } = null!;
    [Inject] private IState<UserState> UserState { get; set; } = null!;

    private bool _statisticsLoaded = false;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UserState.StateChanged += OnUserStateChanged;
        TryLoadStatistics();
    }

    private void OnUserStateChanged(object? sender, EventArgs e)
    {
        TryLoadStatistics();
    }

    private void TryLoadStatistics()
    {
        if (_statisticsLoaded) return;
        if (string.IsNullOrWhiteSpace(UserState.Value.Token)) return;

        _statisticsLoaded = true;
        Dispatcher.Dispatch(new LoadStatisticsAction(
            From: DateTime.UtcNow.AddDays(-30),
            To: DateTime.UtcNow
        ));
    }
}
