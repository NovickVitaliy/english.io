using Fluxor;
using Learning.Features.Practice.Models.ContrastTask;
using Learning.Store.Practice.ContrastTask.Get.Actions;

namespace Learning.Store.Practice.ContrastTask.Get;

public static class ContrastTaskReducers
{
    [ReducerMethod]
    public static ContrastTaskState ReduceGetContrastTaskAction(ContrastTaskState state, GetContrastTaskAction action) => new ContrastTaskState([], true);

    [ReducerMethod]
    public static ContrastTaskState ReduceGetContrastTaskSuccessAction(ContrastTaskState state, GetContrastTaskSuccessAction action)
    {
        List<ContrastTaskUnit> contrastTaskUnits = [];

        foreach (var taskUnit in action.ContrastTask)
        {
            string[] possibleChoices = [..taskUnit.PossibleChoices, taskUnit.CorrectWord];
            Random.Shared.Shuffle(possibleChoices);
            var contrastTaskUnit = taskUnit with
            {
                PossibleChoices = possibleChoices
            };
            contrastTaskUnits.Add(contrastTaskUnit);
        }

        return new ContrastTaskState([.. contrastTaskUnits], false);
    }

    [ReducerMethod]
    public static ContrastTaskState ReduceGetContrastTaskFailureAction(ContrastTaskState state, GetContrastTaskFailureAction action) => new ContrastTaskState([], false);

    [ReducerMethod]
    public static ContrastTaskState ReduceSaveContrastTaskResultAction(ContrastTaskState state, SaveContrastTaskResultAction action)
        => state with
        {
            IsSaving = true
        };

    [ReducerMethod]
    public static ContrastTaskState ReduceSaveContrastTaskResultSuccessAction(ContrastTaskState state, SaveContrastTaskResultSuccessAction action)
        => state with
        {
            IsSaving = false
        };


    [ReducerMethod]
    public static ContrastTaskState ReduceSaveContrastTaskResultFailureAction(ContrastTaskState state, SaveContrastTaskResultFailureAction action)
        => state with
        {
            IsSaving = false
        };
}
