using Fluxor;
using Learning.Features.Practice.Components;
using Learning.Store.Practice.UnknownWords;
using Learning.Store.Practice.UnknownWords.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Learning.Features.Practice.Pages;

public partial class PracticeResult : Fluxor.Blazor.Web.Components.FluxorComponent
{
    [Inject] private IState<UnknownWordsState> UnknownWordsState { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IStringLocalizer<PracticeResult> Localizer { get; init; } = null!;
    [Inject] private IDialogService DialogService { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher {get; init;} = null!;

    private Task<IDialogReference> OpenAddToDeckModal(UnknownWord unknownWord)
    {
        var options = new DialogOptions()
        {
            CloseButton = true, CloseOnEscapeKey = true
        };

        var parameters = new DialogParameters<AddUnknownWordToTheDeckModal>
        {
            {
                x => x.SelectedWord, unknownWord.Word
            }
        };

        return DialogService.ShowAsync<AddUnknownWordToTheDeckModal>(Localizer["Dialog_Name"], parameters, options);
    }

    private Task ToDecks()
    {
        Dispatcher.Dispatch(new ClearUnknownWordsAction());
        NavigationManager.NavigateTo("/learning/decks");
        return Task.CompletedTask;
    }
}

