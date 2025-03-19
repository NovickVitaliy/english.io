using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Decks.Actions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Store.User;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Learning.Features.Decks.Components;

public partial class CreateDeckDialog : ComponentBase
{
    private MudForm _form = null!;
    private CreateDeckRequest _request = new();

    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Inject] private IDecksService DecksService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;

    private void Cancel() => MudDialog.Close(DialogResult.Ok(this));

    private async Task Submit()
    {
        await _form.Validate();

        if (!_form.IsValid) return;

        try
        {
            var token = UserState.Value.Token;
            var deckId = await DecksService.CreateDeckAsync(_request, token);
            Snackbar.Add(Localizer["Deck_Created_Successfully"], Severity.Success);
            Dispatcher.Dispatch(new AddDeckAction(new DeckDto(deckId, string.Empty, _request.DeckTopic, _request.IsStrict, 0)));
            MudDialog.Close(DialogResult.Ok(this));
        }
        catch (ApiException e)
        {
            var problemDetails = e.ToProblemDetails();
            Snackbar.Add(Localizer[problemDetails.Detail ?? "Error_Occured"], Severity.Error);
        }
    }
}

