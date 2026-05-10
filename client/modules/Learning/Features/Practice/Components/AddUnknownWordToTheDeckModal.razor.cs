using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Practice.UnknownWords;
using Learning.Store.Practice.UnknownWords.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.Store.User;

namespace Learning.Features.Practice.Components;

public partial class AddUnknownWordToTheDeckModal : ComponentBase
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Inject] private IDecksService DecksService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IState<UnknownWordsState> UnknownWordsState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IStringLocalizer<AddUnknownWordToTheDeckModal> Localizer { get; init; } = null!;

    [Parameter] public string SelectedWord { get; init; } = string.Empty;

    private Guid? SelectedDeckId = null;

    private bool IsLoading;

    private List<(Guid DeckId, string DeckName)> Decks = [];

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        Decks = (await DecksService.GetDecksForUserAsync(new GetDecksForUserRequest(UserState.Value.Email), UserState.Value.Token)).Decks.Select(x => (x.Id, x.Topic)).ToList();
        StateHasChanged();

        IsLoading = false;
    }

    private void Cancel() => MudDialog.Close(DialogResult.Ok(this));

    private void Save()
    {
        if (!SelectedDeckId.HasValue)
        {
            Snackbar.Add("Please select a deck.", Severity.Error);
            return;
        }

        Dispatcher.Dispatch(new SaveUnknownWord(SelectedDeckId.Value, SelectedWord));

        MudDialog.Close(DialogResult.Ok(this));
    }
}

