using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.ReadingComprehensionTask.Check.Actions;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice.ReadingComprehensionTask.Check;

public class ReadingComprehensionTaskResultEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public ReadingComprehensionTaskResultEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleCheckReadingComprehestionAction(CheckReadingComprehensionTaskAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _practiceService.CheckReadingComprehensionExercise(action.Request, _userState.Value.Token),
            response => new CheckReadingComprehensionTaskSuccessAction(response),
            em => new CheckReadingComprehensionTaskFailureAction(em),
            dispatcher);
    }
}
