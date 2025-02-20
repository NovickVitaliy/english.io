using Fluxor;
using Learning.Features.Decks.Components;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.Store.User;

namespace Learning.Features.Decks.Pages;

public partial class DeckPage : ComponentBase
{
    [Parameter] public Guid DeckId { get; init; }
    [Inject] private IStringLocalizer<DeckPage> Localizer { get; init; } = null!;
    [Inject] private IDialogService DialogService { get; init; } = null!;

    private Task<IDialogReference> ShowAddWordDialog()
    {
        var options = new DialogOptions()
        {
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var parameters = new DialogParameters<AddDeckWordModal> { { x => x.DeckId, DeckId } };

        return DialogService.ShowAsync<AddDeckWordModal>(Localizer["Dialog_Name"], parameters, options);
    }
}

