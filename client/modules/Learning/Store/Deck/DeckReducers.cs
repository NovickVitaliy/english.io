using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Deck.Actions;
using Shared.Store.User;

namespace Learning.Store.Deck;

public static class DeckReducers
{
    [ReducerMethod]
    public static DeckState ReduceFetchDeckAction(DeckState state, FetchDeckAction action) => new DeckState(null, true);

    [ReducerMethod]
    public static DeckState ReduceFetchDeckResultAction(DeckState state, FetchDeckResultAction action) => new DeckState(action.DeckWithWordsDto, false);

    [ReducerMethod]
    public static DeckState ReduceAddDeckWordAction(DeckState state, AddDeckWordAction action)
    {
        var deck = state.DeckWithWordsDto;

        DeckWordDto[] words = [..deck!.DeckWords, action.DeckWordDto];

        return new DeckState(deck with {DeckWords = words, WordCount = words.Length}, false);
    }
}

public class DeckEffects
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
        var deckWithWords = await _decksService.GetDeckAsync(action.DeckId, _userState.Value.Token);
        dispatcher.Dispatch(new FetchDeckResultAction(deckWithWords));
    }
}
