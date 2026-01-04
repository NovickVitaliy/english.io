using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.Actions.FetchWordsForPractice;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice;

public class PracticeEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public PracticeEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleGetWordsForPracticeAction(GetWordsForPracticeAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _practiceService.GetWordsForPracticeAsync(action.DeckId, _userState.Value.Token),
            response => new GetWordsForPracticeSuccessAction(response),
            em => new GetWordsForPracticeFailureAction(em),
            dispatcher);
    }
}
