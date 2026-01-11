using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.Actions;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice;

public class PracticeEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;
    private readonly IState<PracticeState> _practiceState;

    public PracticeEffects(IPracticeService practiceService, IState<UserState> userState, IState<PracticeState> practiceState)
    {
        _practiceService = practiceService;
        _userState = userState;
        _practiceState = practiceState;
    }

    [EffectMethod]
    public async Task HandleGetWordsForPracticeAction(GetWordsForPracticeAction action, IDispatcher dispatcher)
    {
        if (_practiceState.Value.WordsForPractice.Length > 0) return;

        await ProcessRefitApiRequest(
            async () => await _practiceService.GetWordsForPracticeAsync(action.DeckId, _userState.Value.Token),
            response => new GetWordsForPracticeSuccessAction(response),
            em => new GetWordsForPracticeFailureAction(em),
            dispatcher);
    }
}
