using Fluxor;
using Learning.Features.Decks.Models;
using Learning.LearningShared.Services;
using Learning.Store.Decks.Actions.Fetch;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Decks;

public class DecksEffects : BaseEffects
{
    private readonly IDecksService _decksService;
    private readonly IState<UserState> _userState;

    public DecksEffects(IDecksService decksService, IState<UserState> userState)
    {
        _decksService = decksService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleFetchDecksAction(FetchDecksAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _decksService.GetDecksForUserAsync(new GetDecksForUserRequest(action.UserEmail, action.PageNumber, action.PageSize), _userState.Value.Token),
            response => new FetchDecksSuccessAction(response.Decks, response.Count),
            em => new FetchDecksFailureAction(em),
            dispatcher);
    }


}
