using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Deck;
using Learning.Store.Deck.Actions.Fetch;
using Learning.Store.RecentDeck.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.Store.User;

namespace Learning.Features.Decks.Components;

public partial class DeckComponent : FluxorComponent
{
    [Parameter, EditorRequired] public Guid DeckId { get; init; }
    [Inject] private IDecksService DecksService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IDialogService DialogService { get; init; } = null!;
    [Inject] private IState<DeckState> DeckState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private IStringLocalizer<DeckComponent> Localizer { get; init; } = null!;

    protected override void OnInitialized()
    {
        UserState.StateChanged += OnUserStateChanged;
        DeckState.StateChanged += OnDeckStateChanged;

        base.OnInitialized();
    }

    private void OnUserStateChanged(object? sender, EventArgs e)
    {
        GetDeckFromApi();
    }

    private void OnDeckStateChanged(object? sender, EventArgs e)
    {
        var deck = DeckState.Value.DeckWithWordsDto;

        if (deck is null)
            return;

        Dispatcher.Dispatch(new AccessDeckAction(DeckId, deck.Topic));
    }

    protected override Task OnParametersSetAsync()
    {
        GetDeckFromApi();
        return Task.CompletedTask;
    }

    private void GetDeckFromApi()
    {
        if (string.IsNullOrWhiteSpace(UserState.Value.Token)) return;
        Dispatcher.Dispatch(new FetchDeckAction(DeckId));
    }

    private async Task ShowWordDialog(DeckEntryDto entry)
    {
        var parameters = new DialogParameters<WordDialog>
        {
            {
                x => x.DeckEntry, entry
            }
        };

        await DialogService.ShowAsync<WordDialog>(Localizer["Create_Word_Dialog"], parameters, new DialogOptions()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        });
    }

    private async Task ConfirmWordDelete(Guid deckEntryId)
    {
        var parameters = new DialogParameters<DeleteDeckEntryDialog>
        {
            {
                x => x.DeckId, DeckId
            },
            {
                x => x.DeckEntryId, deckEntryId
            }
        };

        await DialogService.ShowAsync<DeleteDeckEntryDialog>(Localizer["Delete_Deck_Entry"], parameters, new DialogOptions()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        });
    }
}
