using Fluxor;
using Learning.Store.Practice.Actions;

namespace Learning.Store.Practice;

public static class PracticeReducers
{
    [ReducerMethod]
    public static PracticeState ReduceGetWordsForPracticeSuccessAction(PracticeState state, GetWordsForPracticeSuccessAction action) =>
        new PracticeState(action.Response.WordsForPractice);
}
