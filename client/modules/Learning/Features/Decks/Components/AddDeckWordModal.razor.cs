using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Deck;
using Learning.Store.Deck.Actions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Store.User;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Learning.Features.Decks.Components;

public partial class AddDeckWordModal : ComponentBase
{
    private MudForm? _form = null!;
    private readonly CreateDeckWordRequest _request = new CreateDeckWordRequest();

    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Inject] private IDecksService DecksService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IState<DeckState> DeckState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Parameter] public Guid DeckId { get; init; }
    private void Cancel() => MudDialog.Close(DialogResult.Ok(this));

    private async Task Submit()
    {
        await _form!.Validate();

        try
        {
            var deckWordDto = await DecksService.CreateDeckWordAsync(DeckId, _request, UserState.Value.Token);
            Snackbar.Add(Localizer["Api_Success"], Severity.Success);
            MudDialog.Close(DialogResult.Ok(this));
            Dispatcher.Dispatch(new AddDeckWordAction(deckWordDto));
        }
        catch (ApiException e)
        {
            var problemDetails = e.ToProblemDetails();
            Snackbar.Add(Localizer[problemDetails.Detail ?? "Error_Occured"], Severity.Error);
        }
    }
}

