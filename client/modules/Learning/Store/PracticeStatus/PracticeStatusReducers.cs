using Fluxor;
using Learning.Store.PracticeStatus.Actions;

namespace Learning.Store.PracticeStatus;

public static class PracticeStatusReducers
{
    [ReducerMethod]
    public static PracticeStatusState SetFirstTaskPercentageSuccess(PracticeStatusState state, SetFirstTaskPercentageSuccessAction action) => state with
    {
        FirstTaskPercentageSuccess = action.Percentage
    };

    [ReducerMethod]
    public static PracticeStatusState SetSecondTaskPercentageSuccess(PracticeStatusState state, SetSecondTaskPercentageSuccessAction action) => state with
    {
        SecondTaskPercentageSuccess = action.Percentage
    };

    [ReducerMethod]
    public static PracticeStatusState SetThirdTaskPercentageSuccess(PracticeStatusState state, SetThirdTaskPercentageSuccessAction action) => state with
    {
        ThirdTaskPercentageSuccess = action.Percentage
    };

    [ReducerMethod]
    public static PracticeStatusState SetFourhtTaskPercentageSUccess(PracticeStatusState state, SetFourthTaskPercentageSuccessAction action) => state with
    {
        FourthTaskPercentageSuccess = action.Percentage
    };
}
