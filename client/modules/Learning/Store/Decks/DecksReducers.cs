using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Decks.Actions;
using Shared.Store.User;

namespace Learning.Store.Decks;

public static class DecksReducers
{
    [ReducerMethod]
    public static DecksState ReduceFetchDecksAction(DecksState state, FetchDecksAction action) => new DecksState(null, 0, true);

    [ReducerMethod]
    public static DecksState ReduceAddDeckToState(DecksState state, AddDeckAction action)
    {
        var decks = state.Decks;

        return new DecksState([..decks!, action.Deck], state.Count + 1, false);
    }

    [ReducerMethod]
    public static DecksState ReduceFetchDecksResultAction(DecksState state, FetchDecksResultAction action)
    {
        return new DecksState(action.Decks, action.Count, false);
    }
}

public class Effects
{
    private readonly IDecksService _decksService;
    private readonly IState<UserState> _userState;

    public Effects(IDecksService decksService, IState<UserState> userState)
    {
        _decksService = decksService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleFetchDecksAction(FetchDecksAction action, IDispatcher dispatcher)
    {
        var decks = await _decksService.GetDecksForUserAsync(new GetDecksForUserRequest(action.UserEmail, action.PageNumber, action.PageSize), _userState.Value.Token);
        dispatcher.Dispatch(new FetchDecksResultAction(decks.Decks, decks.Count));
    }
}
