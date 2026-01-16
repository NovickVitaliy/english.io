using Fluxor;
using Learning.Store.Practice.FillInTheGapsTask.Get.Actions;

namespace Learning.Store.Practice.FillInTheGapsTask.Get;

public static class FillInTheGapsTaskReducers
{
    [ReducerMethod]
    public static FillInTheGapsTaskState ReduceGetFillInTheGapsTaskAction(FillInTheGapsTaskState state, GetFillInTheGapsTaskAction action)
        => new FillInTheGapsTaskState([], true);

    [ReducerMethod]
    public static FillInTheGapsTaskState ReduceGetFillInTheGapsTaskSuccessAction(FillInTheGapsTaskState state, GetFillInTheGapsTaskSuccessAction action)
        => new FillInTheGapsTaskState(action.SentencesWithGap, false);

    [ReducerMethod]
    public static FillInTheGapsTaskState ReduceGetFillInTheGapsTaskFailureAction(FillInTheGapsTaskState state, GetFillInTheGapsTaskFailureAction action)
        => new FillInTheGapsTaskState([], false);
}
