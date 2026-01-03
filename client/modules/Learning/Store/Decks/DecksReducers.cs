using Fluxor;
using Learning.Store.Deck.Actions.Create;
using Learning.Store.Decks.Actions.Fetch;

namespace Learning.Store.Decks;

public static class DecksReducers
{
    [ReducerMethod]
    public static DecksState ReduceFetchDecksAction(DecksState state, FetchDecksAction action) => new DecksState(null, 0, true);

    [ReducerMethod]
    public static DecksState ReduceAddDeckToState(DecksState state, CreateDeckSuccessAction successAction)
    {
        var decks = state.Decks;

        return new DecksState([..decks!, successAction.Deck], state.Count + 1, false);
    }

    [ReducerMethod]
    public static DecksState ReduceFetchDecksResultAction(DecksState state, FetchDecksSuccessAction action)
    {
        return new DecksState(action.Decks, action.Count, false);
    }
}
