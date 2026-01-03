using Fluxor;
using Learning.Store.DeckWords.Actions.Delete;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Learning.Features.Decks.Components;

public partial class DeleteDeckEntryDialog : ComponentBase
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Inject] private IStringLocalizer<DeleteDeckEntryDialog> Localizer { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Parameter] public Guid DeckId { get; init; }
    [Parameter] public Guid DeckEntryId { get; init; }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private Task Submit()
    {
        Dispatcher.Dispatch(new DeleteDeckEntryAction(DeckId, DeckEntryId));
        MudDialog.Close(DialogResult.Ok(this));

        return Task.CompletedTask;
    }
}

