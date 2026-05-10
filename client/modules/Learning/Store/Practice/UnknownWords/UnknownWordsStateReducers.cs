using Fluxor;
using Learning.Store.Practice.UnknownWords.Actions;

namespace Learning.Store.Practice.UnknownWords;

public static class UnknownWordsStateReducers
{
    [ReducerMethod]
    public static UnknownWordsState ReducerAddUnknownWordAction(UnknownWordsState state, AddUnknownWordAction action) => new UnknownWordsState([..state.UnknownWords, new UnknownWord(action.Word)], false);

    [ReducerMethod]
    public static UnknownWordsState ReduceSaveUnknownWordAction(UnknownWordsState state, SaveUnknownWord action)
    {

        return state with
        {
            IsSaving = true
        };
    }

    [ReducerMethod]
    public static UnknownWordsState ReduceSaveUnknownWordSuccessAction(UnknownWordsState state, SaveUnknownWordSuccessAction action)
    {
        var words = state.UnknownWords;
        foreach (var unknownWord in words)
        {
            if (unknownWord.Word == action.Word)
            {
                unknownWord.IsSaved = true;
            }
        }

        return new UnknownWordsState(IsSaving: false, UnknownWords: words);
    }

    [ReducerMethod]
    public static UnknownWordsState ReduceSaveUnknownWordFailureAction(UnknownWordsState state, SaveUnknownWordFailureAction action)
    {
        return state with
        {
            IsSaving = false
        };
    }

    [ReducerMethod]
    public static UnknownWordsState ReduceClearUnknownWordStateAction(UnknownWordsState state, ClearUnknownWordsAction action)
    {
        return new UnknownWordsState([], false);
    }
}
