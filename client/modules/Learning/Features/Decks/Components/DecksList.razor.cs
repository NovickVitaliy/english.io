using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Microsoft.AspNetCore.Components;
using Shared.Store.User;

namespace Learning.Features.Decks.Components;

public partial class DecksList : ComponentBase
{
    private DeckDto[]? _decks = null!;
    private int _currentPage = 1;
    private double _totalPageCount;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IDecksService DecksService { get; init; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        await GetDecksFromApi();
    }

    private async Task GetDecksFromApi()
    {
        var request = new GetDecksForUserRequest(UserState.Value.Email, _currentPage);
        Console.WriteLine(request);
        var response = await DecksService.GetDecksForUserAsync(request, UserState.Value.Token);
        _decks = response.Decks;
        _totalPageCount = Math.Ceiling((double)(response.Count / request.PageSize));
    }

    private async Task ChangePage(int page)
    {
        _currentPage = page;
        await GetDecksFromApi();
    }
}

