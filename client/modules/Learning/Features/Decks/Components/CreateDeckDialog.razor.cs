using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Deck.Actions.Create;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Learning.Features.Decks.Components;

public partial class CreateDeckDialog : ComponentBase
{
    private MudForm _form = null!;
    private CreateDeckRequest _request = new();

    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;

    private void Cancel() => MudDialog.Close(DialogResult.Ok(this));

    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            Dispatcher.Dispatch(new CreateDeckAction(_request));
            MudDialog.Close(DialogResult.Ok(this));
        }
    }
}
