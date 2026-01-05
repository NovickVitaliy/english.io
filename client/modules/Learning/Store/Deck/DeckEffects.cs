using Fluxor;
using Learning.Features.Decks.Components;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Deck.Actions.Create;
using Learning.Store.Deck.Actions.Delete;
using Learning.Store.Deck.Actions.Fetch;
using Learning.Store.Deck.Actions.Import;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Deck;

public class DeckEffects : BaseEffects
{
    private readonly IDecksService _decksService;
    private readonly IState<UserState> _userState;
    private readonly ISnackbar _snackbar;
    private readonly IStringLocalizer<DecksToolbar> _localizer;

    public DeckEffects(IDecksService decksService, IState<UserState> userState, ISnackbar snackbar, IStringLocalizer<DecksToolbar> localizer)
    {
        _decksService = decksService;
        _userState = userState;
        _snackbar = snackbar;
        _localizer = localizer;
    }

    [EffectMethod]
    public async Task HandleFetchDeckAction(FetchDeckAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _decksService.GetDeckAsync(action.DeckId, _userState.Value.Token),
            response => new FetchDeckSuccessAction(response),
            errorMessage => new FetchDeckFailureAction(errorMessage),
            dispatcher);
    }

    [EffectMethod]
    public async Task HandleCreateDeckAction(CreateDeckAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _decksService.CreateDeckAsync(action.Request, _userState.Value.Token),
            response => new CreateDeckSuccessAction(new DeckDto(response, string.Empty, action.Request.DeckTopic, action.Request.IsStrict, 0)),
            em => new CreateDeckFailureAction(em),
            dispatcher);
    }

    [EffectMethod]
    public async Task HandleDeleteDeckAction(DeleteDeckAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequestWithNoResponse(
            async () => await _decksService.DeleteDeckAsync(action.DeckId, _userState.Value.Token),
            () => new DeleteDeckSuccessAction(action.DeckId),
            em => new DeleteDeckFailureAction(em),
            dispatcher);
    }

    [EffectMethod]
    public async Task HandleImportDeckAction(ImportDeckAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _decksService.ImportDeckAsync(action.StreamPart, _userState.Value.Token),
            response => new ImportDeckSuccessAction(response),
            em => new ImportDeckFailureAction(em),
            dispatcher);
    }

    [EffectMethod]
    public Task HandleImportDeckSuccessAction(ImportDeckSuccessAction action, IDispatcher dispatcher)
    {
        _snackbar.Add(_localizer["Import_Successfull"], Severity.Success);
        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleImportDeckFailureAction(ImportDeckFailureAction action, IDispatcher dispatcher)
    {
        _snackbar.Add(_localizer["Import_Unsuccessfull"], Severity.Error);
        return Task.CompletedTask;
    }
}
