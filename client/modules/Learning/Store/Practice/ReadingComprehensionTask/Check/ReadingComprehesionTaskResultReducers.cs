using Fluxor;
using Learning.Store.Practice.ReadingComprehensionTask.Check.Actions;

namespace Learning.Store.Practice.ReadingComprehensionTask.Check;

public static class ReadingComprehesionTaskResultReducers
{
    [ReducerMethod]
    public static ReadingComprehesionTaskResultState ReduceCheckReadingComprehenstionExerciceAction(ReadingComprehesionTaskResultState state, CheckReadingComprehensionTaskAction action)
        => new ReadingComprehesionTaskResultState(null, true);

    [ReducerMethod]
    public static ReadingComprehesionTaskResultState ReduceCheckReadingComprehenstionExerciceSuccessAction(ReadingComprehesionTaskResultState state, CheckReadingComprehensionTaskSuccessAction action)
        => new ReadingComprehesionTaskResultState(action.Result, false);

    [ReducerMethod]
    public static ReadingComprehesionTaskResultState ReduceCheckReadingComprehenstionExerciceFailureAction(ReadingComprehesionTaskResultState state, CheckReadingComprehensionTaskFailureAction action)
        => new ReadingComprehesionTaskResultState(null, false);
}
