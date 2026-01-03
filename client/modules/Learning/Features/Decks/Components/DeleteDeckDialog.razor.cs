using Fluxor;
using Learning.Store.Deck.Actions.Delete;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Learning.Features.Decks.Components;

public partial class DeleteDeckDialog : ComponentBase
{
    [Parameter] public Guid DeckId { get; init; }
    [Inject] private IStringLocalizer<DeleteDeckDialog> Localizer { get; init; } = null!;
    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;

    private void Cancel() => MudDialog.Cancel();

    private void Submit()
    {
        Dispatcher.Dispatch(new DeleteDeckAction(DeckId));
        MudDialog.Close(DialogResult.Ok(this));
    }
}
