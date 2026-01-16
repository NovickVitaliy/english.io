using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.FillInTheGapsTask.Check.Actions;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice.FillInTheGapsTask.Check;

public class FillInTheGapsTaskResultEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public FillInTheGapsTaskResultEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleCheckFillInTheGapsTaskAction(CheckFillInTheGapsTaskAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _practiceService.CheckFillInTheGapsTaskAsync(action.Request, _userState.Value.Token),
            response => new CheckFillInTheGapsTaskSuccessAction(response),
            em => new CheckFillInTheGapsTaskFailureAction(em),
            dispatcher);
    }
}
