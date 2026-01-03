using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Deck.Actions.Create;
using Learning.Store.Deck.Actions.Delete;
using Learning.Store.Deck.Actions.Fetch;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Deck;

public class DeckEffects : BaseEffects
{
    private readonly IDecksService _decksService;
    private readonly IState<UserState> _userState;

    public DeckEffects(IDecksService decksService, IState<UserState> userState)
    {
        _decksService = decksService;
        _userState = userState;
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
}
