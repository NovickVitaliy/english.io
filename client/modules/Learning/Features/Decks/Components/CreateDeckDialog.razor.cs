using Learning.Features.Decks.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Learning.Features.Decks.Components;

public partial class CreateDeckDialog : ComponentBase
{
    private MudForm _form = null!;
    private CreateDeckRequest _request = new();

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; init; } = null!;

    private void Cancel() => MudDialog.Close(DialogResult.Ok(this));

    private Task Submit()
    {
        Console.WriteLine(_request.DeckTopic);
        Console.WriteLine(_request.IsStrict);
        return Task.CompletedTask;
    }
}

