using System.ComponentModel.DataAnnotations;
using Learning.Features.Decks.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Learning.Features.Decks.Components;

public partial class DeckListItem : ComponentBase
{
    [Parameter, EditorRequired] public DeckDto DeckDto { get; init; } = null!;
    [Inject] private IStringLocalizer<DeckListItem> Localizer { get; init; } = null!;
    [Inject] private IDialogService DialogService { get; init; } = null!;

    private Task<IDialogReference> OpenConfirmDeleteDialog()
    {
        var options = new DialogOptions()
        {
            CloseButton = true, CloseOnEscapeKey = true
        };

        var parameters = new DialogParameters<DeleteDeckDialog>
        {
            {
                x => x.DeckId, DeckDto.Id
            }
        };

        return DialogService.ShowAsync<DeleteDeckDialog>(Localizer["Title"], parameters: parameters, options: options);
    }
}
