using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Store.User;

namespace Learning.Features.Decks.Components;

public partial class DeckComponent : ComponentBase
{
    private DeckWithWordsDto? _deckDto = new DeckWithWordsDto(
        Guid.NewGuid(),
        "123",
        "test",
        true,
        5,
        [
            new DeckWordDto(Guid.NewGuid(), "ua version", "eng version", ["first", "second", "third"])
        ]);

    [Parameter, EditorRequired] public Guid DeckId { get; init; }
    [Inject] private IDecksService DecksService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IDialogService DialogService { get; init; } = null!;

    // protected override async Task OnParametersSetAsync()
    // {
    //     if (UserState.Value is not null)
    //     {
    //         _deckDto = await DecksService.GetDeckAsync(DeckId, UserState.Value.Token);
    //     }
    // }

    private async Task ShowWordDialog(DeckWordDto word)
    {
        var parameters = new DialogParameters<WordDialog> { { x => x.DeckWord, word } };

        var dialog = await DialogService.ShowAsync<WordDialog>("Delete Server", parameters);
    }
}

