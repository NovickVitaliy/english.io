using Fluxor;
using Learning.Store.Practice.Actions;

namespace Learning.Store.Practice;

public static class PracticeReducers
{
    [ReducerMethod]
    public static PracticeState SetWordsBeingPracticed(PracticeState state, SetWordsBeingPracticedAction action) => new PracticeState(WordsBeingPracticed: action.Words);
}
