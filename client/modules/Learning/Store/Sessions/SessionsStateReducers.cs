using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Features.Settings.Models.Sessions;
using Learning.Store.Sessions.Actions;
using Shared.Store.User;

namespace Learning.Store.Sessions;

public static class SessionsStateReducers
{
    [ReducerMethod]
    public static SessionsState ReduceFetchSessionsAction(SessionsState state, FetchSessionsAction action) => new SessionsState(null, 0, true);

    [ReducerMethod]
    public static SessionsState ReduceFetchSessionsResultAction(SessionsState state, FetchSessionsResultAction action) => new SessionsState(action.Sessions, action.Count, false);
}

public class SessionsEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public SessionsEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleFetchSessionsAction(FetchSessionsAction action, IDispatcher dispatcher)
    {
        var result = await _practiceService.FetchSessionForUserAsync(new GetSessionResultsForUserRequest(action.CurrentPage, action.PageSize, _userState.Value.Email), _userState.Value.Token);
        dispatcher.Dispatch(new FetchSessionsResultAction(result.Sessions, result.Count));
    }
}
