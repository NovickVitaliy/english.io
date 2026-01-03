using Fluxor;
using Learning.Features.Decks.Models;
using Learning.Store.Deck.Actions.Delete;
using Learning.Store.Deck.Actions.Fetch;
using Learning.Store.Decks;
using Learning.Store.DeckWords.Actions;
using Learning.Store.DeckWords.Actions.Delete;

namespace Learning.Store.Deck;

public static class DeckReducers
{
    [ReducerMethod]
    public static DeckState ReduceFetchDeckAction(DeckState state, FetchDeckAction action) => new DeckState(null, true);

    [ReducerMethod]
    public static DeckState ReduceFetchDeckResultAction(DeckState state, FetchDeckSuccessAction action) => new DeckState(action.DeckWithWordsDto, false);

    [ReducerMethod]
    public static DeckState ReduceAddDeckWordAction(DeckState state, CreateDeckWordSuccessAction successAction)
    {
        var deck = state.DeckWithWordsDto;

        DeckEntryDto[] words = [..deck!.DeckWords, successAction.DeckEntryDto];

        return new DeckState(deck with
        {
            DeckWords = words, WordCount = words.Length
        }, false);
    }

    [ReducerMethod]
    public static DecksState ReduceRemoveDeckAction(DecksState state, DeleteDeckSuccessAction successAction)
    {
        var decks = state.Decks!
            .Where(x => x.Id != successAction.DeckId)
            .ToArray();

        return new DecksState(decks, decks.Length, false);
    }

    [ReducerMethod]
    public static DeckState ReduceRemoteDeckEntryAction(DeckState state, DeleteDeckEntrySuccessAction action)
    {
        var deck = state.DeckWithWordsDto;

        DeckEntryDto[] words = deck!.DeckWords.Where(x => x.Id != action.EntryWordId).ToArray();

        return new DeckState(deck with
        {
            DeckWords = words, WordCount = words.Length
        }, false);
    }
}
