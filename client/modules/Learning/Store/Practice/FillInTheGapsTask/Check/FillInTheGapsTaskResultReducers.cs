using Fluxor;
using Learning.Store.Practice.FillInTheGapsTask.Check.Actions;

namespace Learning.Store.Practice.FillInTheGapsTask.Check;

public static class FillInTheGapsTaskResultReducers
{
    [ReducerMethod]
    public static FillInTheGapsTaskResultState ReduceCheckFillInTheGapsTaskAction(FillInTheGapsTaskResultState state, CheckFillInTheGapsTaskAction action)
        => new FillInTheGapsTaskResultState([], true);

    [ReducerMethod]
    public static FillInTheGapsTaskResultState ReduceCheckFillInTheGapsTaskSuccessAction(FillInTheGapsTaskResultState state, CheckFillInTheGapsTaskSuccessAction action)
        => new FillInTheGapsTaskResultState(action.SentenceWithFilledGapResults, false);

    [ReducerMethod]
    public static FillInTheGapsTaskResultState ReduceCheckFillInTheGapsTaskFailureAction(FillInTheGapsTaskResultState state, CheckFillInTheGapsTaskFailureAction action)
        => new FillInTheGapsTaskResultState([], false);
}
