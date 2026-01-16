using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.FillInTheGapsTask.Get.Actions;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice.FillInTheGapsTask.Get;

public class FillInTheGapsTaskEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public FillInTheGapsTaskEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleGetFillInTheGapsTaskAction(GetFillInTheGapsTaskAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _practiceService.GetFillInTheGapsTaskAsync(action.DeckId, action.WordsForPractice, _userState.Value.Token),
            response => new GetFillInTheGapsTaskSuccessAction(response),
            em => new GetFillInTheGapsTaskFailureAction(em),
            dispatcher);
    }
}
