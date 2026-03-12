using Fluxor;
using Learning.Store.Practice.ReadingComprehensionTask.Get.Actions;

namespace Learning.Store.Practice.ReadingComprehensionTask.Get;

public static class ReadingComprehensionTaskReducers
{
    [ReducerMethod]
    public static ReadingComprehensionTaskState ReduceGetReadingComprehesionTaskAction(ReadingComprehensionTaskState state, GetReadingComprehesionTaskAction action)
        => new ReadingComprehensionTaskState(null, true);

    [ReducerMethod]
    public static ReadingComprehensionTaskState ReduceGetReadingComprehesionTaskSuccessAction(ReadingComprehensionTaskState state, GetReadingComprehensionTaskSuccessAction action)
        => new ReadingComprehensionTaskState(action.ReadingComprehensionExercise, false);

    [ReducerMethod]
    public static ReadingComprehensionTaskState ReduceGetReadingComprehesionTaskFailureAction(ReadingComprehensionTaskState state, GetReadingComprehensionTaskFailureAction action)
        => new ReadingComprehensionTaskState(null, false);
}
