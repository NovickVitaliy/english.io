using Fluxor;
using Learning.LearningShared.Services;
using Learning.Store.Decks.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Store.User;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Learning.Features.Decks.Components;

public partial class DeleteDeckDialog : ComponentBase
{
    [Parameter] public Guid DeckId { get; init; }
    [Inject] private IStringLocalizer<DeleteDeckDialog> Localizer { get; init; } = null!;
    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Inject] private IDecksService DecksService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;

    private void Cancel() => MudDialog.Cancel();

    private void Submit()
    {
        try
        {
            DecksService.DeleteDeckAsync(DeckId, UserState.Value.Token);
            Snackbar.Add(@Localizer["Deleted_Successfully"], Severity.Success);
            Dispatcher.Dispatch(new RemoveDeckAction(DeckId));
            MudDialog.Close(DialogResult.Ok(this));
        }
        catch (ApiException e)
        {
            ProblemDetails problemDetails = e.ToProblemDetails();
            Snackbar.Add(problemDetails.Title ?? problemDetails.Detail ?? "Error occured", Severity.Error);
        }
    }
}
