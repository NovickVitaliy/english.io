using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Microsoft.AspNetCore.Components;
using Shared.Store.User;

namespace Learning.Features.Decks.Components;

public partial class DecksList : ComponentBase
{
    private DeckDto[]? _decks = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IDecksService DecksService { get; init; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _decks = await DecksService.GetDecksForUserAsync(UserState.Value.Email);
    }
}

