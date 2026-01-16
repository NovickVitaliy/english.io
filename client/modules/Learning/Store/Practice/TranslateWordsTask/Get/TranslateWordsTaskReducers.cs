using Fluxor;
using Learning.Store.Practice.TranslateWordsTask.Get.Actions;

namespace Learning.Store.Practice.TranslateWordsTask.Get;

public static class TranslateWordsTaskReducers
{
    [ReducerMethod]
    public static TranslateWordsTaskState ReduceGetTranslateTaskSuccessAction(TranslateWordsTaskState taskState, GetTranslationTaskSuccessAction action) => new TranslateWordsTaskState(action.Words, false);
}
