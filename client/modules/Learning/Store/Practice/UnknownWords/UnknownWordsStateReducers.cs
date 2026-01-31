using Fluxor;
using Learning.Store.Practice.UnknownWords.Actions;

namespace Learning.Store.Practice.UnknownWords;

public static class UnknownWordsStateReducers
{
    [ReducerMethod]
    public static UnknownWordsState ReducerAddUnknownWordAction(UnknownWordsState state, AddUnknownWordAction action) => new UnknownWordsState([..state.UnknownWords, action.Word], false);
}
