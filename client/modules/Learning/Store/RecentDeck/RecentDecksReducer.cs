using Fluxor;
using Learning.Store.RecentDeck.Actions;

namespace Learning.Store.RecentDeck;

public static class RecentDecksReducers
{
    [ReducerMethod]
    public static RecentDecksState OnAccessDeck(RecentDecksState state, AccessDeckAction action)
    {
        var updated = state.RecentDecks
            .Where(d => d.Id != action.Id)
            .ToList();

        updated.Insert(0, new RecentDeckEntry(action.Id, action.Name, DateTime.Now));

        return state with
        {
            RecentDecks = updated.Take(10).ToList()
        };
    }
}
