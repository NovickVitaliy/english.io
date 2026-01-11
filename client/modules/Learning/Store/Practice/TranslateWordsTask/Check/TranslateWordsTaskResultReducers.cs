using Fluxor;
using Learning.Store.Practice.TranslateWordsTask.Check.Actions;

namespace Learning.Store.Practice.TranslateWordsTask.Check;

public static class TranslateWordsTaskResultReducers
{
    [ReducerMethod]
    public static TranslateWordsTaskResultState ReduceCheckTranslateWordsTaskAction(TranslateWordsTaskResultState state, CheckTranslateWordsTaskAction action) =>
        new TranslateWordsTaskResultState([], true);

    [ReducerMethod]
    public static TranslateWordsTaskResultState ReduceCheckTranslateWordsTaskSuccessAction(TranslateWordsTaskResultState state, CheckTranslateWordsTaskSuccessAction action) =>
        new TranslateWordsTaskResultState(action.Results, false);

    [ReducerMethod]
    public static TranslateWordsTaskResultState ReduceCheckTranslateWordsTaskаFailureAction(TranslateWordsTaskResultState state, CheckTranslateWordsTaskFailureAction action) =>
        new TranslateWordsTaskResultState([], false);

    [ReducerMethod]
    public static TranslateWordsTaskResultState ReduceResetTranslateWordsTaskAction(TranslateWordsTaskResultState state, ResetTranslateWordsTaskAction action) =>
        new TranslateWordsTaskResultState([], false);
}
