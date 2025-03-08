using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Decks;
using Learning.Store.Decks.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Shared.Store.User;

namespace Learning.Features.Decks.Components;

public partial class DecksList : FluxorComponent
{
    private int _currentPage = 1;
    private const int PageSize = 10;
    private double _totalPageCount;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IState<DecksState> DecksState { get; init; } = null!;
    [Inject] private IDecksService DecksService { get; init; } = null!;
    [Inject] private IStringLocalizer<DecksList> Localizer { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;

    protected override void OnInitialized()
    {
        DecksState.StateChanged += (_, _) =>
        {
            _totalPageCount = Math.Ceiling((double)(DecksState.Value.Count / PageSize));
        };

        UserState.StateChanged += (_, _) =>
        {
            GetDecksFromApi();
        };

        base.OnInitialized();
    }

    protected override async Task OnParametersSetAsync()
    {
        await GetDecksFromApi();
    }

    private Task GetDecksFromApi()
    {
        if (string.IsNullOrWhiteSpace(UserState.Value.Token)) return Task.CompletedTask;
        Dispatcher.Dispatch(new FetchDecksAction(UserState.Value.Email, _currentPage, PageSize));
        return Task.CompletedTask;
    }

    private async Task ChangePage(int page)
    {
        _currentPage = page;
        await GetDecksFromApi();
    }
}

