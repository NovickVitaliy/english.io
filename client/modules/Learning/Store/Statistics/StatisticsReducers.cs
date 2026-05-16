using Fluxor;

namespace Learning.Store.Statistics;

public static class StatisticsReducers
{
    [ReducerMethod]
    public static StatisticsState OnLoad(StatisticsState state, LoadStatisticsAction _) =>
        state with { IsLoading = true, ErrorMessage = null };

    [ReducerMethod]
    public static StatisticsState OnSuccess(StatisticsState state, LoadStatisticsSuccessAction action) =>
        state with { IsLoading = false, Statistics = action.Statistics };

    [ReducerMethod]
    public static StatisticsState OnFailure(StatisticsState state, LoadStatisticsFailureAction action) =>
        state with { IsLoading = false, ErrorMessage = action.ErrorMessage };
}
