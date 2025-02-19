using Learning.Features.Decks.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Learning.Features.Decks.Components;

public partial class WordDialog : ComponentBase
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; init; } = null!;
    [Parameter] public DeckWordDto DeckWord { get; init; } = null!;
    [Inject] private IStringLocalizer<WordDialog> Localizer { get; init; } = null!;

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));
}

