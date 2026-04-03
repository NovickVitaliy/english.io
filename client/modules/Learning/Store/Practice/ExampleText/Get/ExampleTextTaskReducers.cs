using Fluxor;
using Learning.Store.Practice.ExampleText.Get.Actions;

namespace Learning.Store.Practice.ExampleText.Get;

public static class ExampleTextTaskReducers
{
    [ReducerMethod]
    public static ExampleTextTaskState ReduceGetExampleTextTaskAction(ExampleTextTaskState state, GetExampleTextTaskAction action)
        => new ExampleTextTaskState(string.Empty, true);

    [ReducerMethod]
    public static ExampleTextTaskState ReduceGetExampleTextTaskSuccessAction(ExampleTextTaskState state, GetExampleTextTaskSuccessAction action)
        => new ExampleTextTaskState(action.Text, false);

    [ReducerMethod]
    public static ExampleTextTaskState ReduceGetExampleTextTaskFailureAction(ExampleTextTaskState state, GetExampleTextTaskFailureAction action)
        => new ExampleTextTaskState(string.Empty, false);
}
