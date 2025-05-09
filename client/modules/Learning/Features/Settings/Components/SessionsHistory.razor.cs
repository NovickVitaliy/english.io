using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Store.Sessions;
using Learning.Store.Sessions.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.Store.User;

namespace Learning.Features.Settings.Components;

public partial class SessionsHistory : FluxorComponent
{
    private int _currentPage = 1;
    private const int PageSize = 10;
    private double _totalPageCount;
    [Inject] private IStringLocalizer<SessionsHistory> Localizer { get; init; } = null!;
    [Inject] private IState<SessionsState> SessionsState { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;

    protected override void OnInitialized()
    {
        SessionsState.StateChanged += (_, _) =>
        {
            _totalPageCount = Math.Ceiling((double)SessionsState.Value.Count / PageSize);
        };

        UserState.StateChanged += async (_, _) =>
        {
            await FetchSessionsFromApi();
        };

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        await FetchSessionsFromApi();
        await base.OnInitializedAsync();
    }

    private Task FetchSessionsFromApi()
    {
        if(string.IsNullOrWhiteSpace(UserState.Value.Token)) return Task.CompletedTask;
        Dispatcher.Dispatch(new FetchSessionsAction(UserState.Value.Email, _currentPage, PageSize));
        return Task.CompletedTask;
    }

    private async Task ChangePage(int page)
    {
        _currentPage = page;
        await FetchSessionsFromApi();
    }

    private static Color GetColorBasedOnSuccessPercentage(double taskPercentageSuccess)
    {
        return taskPercentageSuccess switch
        {
            >= 75 => Color.Success,
            >= 50 and < 75 => Color.Warning,
            < 50 => Color.Error,
            _ => Color.Default
        };
    }
}

