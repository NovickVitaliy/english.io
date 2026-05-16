using Fluxor;
using Learning.Features.Home.Services;
using Shared.Store.User;

namespace Learning.Store.Statistics;

public class StatisticsEffects
{
    private readonly IStatisticsApi _statisticsApi;
    private readonly IState<UserState> _userState;

    public StatisticsEffects(IStatisticsApi statisticsApi, IState<UserState> userState)
    {
        _statisticsApi = statisticsApi;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleLoad(LoadStatisticsAction action, IDispatcher dispatcher)
    {
        try
        {
            Console.WriteLine($"Token: {_userState.Value.Token}");
            var result = await _statisticsApi.GetStatisticsAsync(_userState.Value.Token, action.From, action.To);
            dispatcher.Dispatch(new LoadStatisticsSuccessAction(result));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadStatisticsFailureAction(ex.Message));
        }
    }
}
