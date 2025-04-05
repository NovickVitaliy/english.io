using Fluxor;
using Learning.Store.Practice.Actions;

namespace Learning.Store.Practice;

public static class PracticeReducers
{
    [ReducerMethod]
    public static TranslateWordsState SetWordsBeingPracticed(TranslateWordsState state, SetWordsBeingPracticedAction action) => new TranslateWordsState(WordsBeingPracticed: action.Words);

    [ReducerMethod]
    public static FIllInTheGapsState SetFillInTheGapsWords(FIllInTheGapsState state, SetWordsForFillInTheGapsPracticeAction action) => new FIllInTheGapsState(action.Words);
}
