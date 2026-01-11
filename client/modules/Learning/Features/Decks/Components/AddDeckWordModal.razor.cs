using Fluxor;
using Learning.Features.Decks.Models;
using Learning.Store.DeckWords.Actions.Create;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Learning.Features.Decks.Components;

public partial class AddDeckWordModal : ComponentBase
{
    private MudForm? _form = null!;
    private readonly CreateDeckWordRequest _request = new CreateDeckWordRequest();

    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Parameter] public Guid DeckId { get; init; }

    private void Cancel() => MudDialog.Close(DialogResult.Ok(this));

    private async Task Submit()
    {
        await _form!.Validate();
        Dispatcher.Dispatch(new CreateDeckWordAction(DeckId, _request));
        MudDialog.Close(DialogResult.Ok(this));
    }
}
