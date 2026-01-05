using Fluxor;
using Learning.Store.Deck.Actions.Import;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Refit;

namespace Learning.Features.Decks.Components;

public partial class DecksToolbar : ComponentBase
{
    private string _searchQuery = "";

    [Inject] private IDialogService DialogService { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;

    private Task<IDialogReference> ShowCreateDeckDialog()
    {
        var options = new DialogOptions()
        {
            CloseButton = true, CloseOnEscapeKey = true
        };

        return DialogService.ShowAsync<CreateDeckDialog>(Localizer["Dialog_Name"], options);
    }

    private Task HandleFilesChanged(IBrowserFile? file)
    {
        if (file is null) return Task.CompletedTask;

        var stream = file.OpenReadStream(10 * 1024 * 1024);

        Dispatcher.Dispatch(new ImportDeckAction(new StreamPart(
            stream,
            file.Name,
            file.ContentType)));

        return Task.CompletedTask;
    }
}
