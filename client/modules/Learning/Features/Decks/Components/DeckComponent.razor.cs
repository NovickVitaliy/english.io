using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Microsoft.AspNetCore.Components;
using Shared.Store.User;

namespace Learning.Features.Decks.Components;

public partial class DeckComponent : ComponentBase
{
    private DeckDto? _deckDto;

    public Guid DeckId { get; init; }
    [Inject] private IDecksService DecksService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        if (UserState.Value is not null)
        {
            _deckDto = await DecksService.GetDeckAsync(DeckId, UserState.Value.Token);
        }
    }
}

