using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.ReadingComprehensionTask.Get.Actions;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice.ReadingComprehensionTask.Get;

public class ReadingComprehensionTaskEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public ReadingComprehensionTaskEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleGetReadingComprehesionTaskAction(GetReadingComprehesionTaskAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _practiceService.GetReadingComprehensionExercise(action.DeckId, action.WordsForPractice, _userState.Value.Token),
            response => new GetReadingComprehensionTaskSuccessAction(response),
            em => new GetReadingComprehensionTaskFailureAction(em),
            dispatcher);
    }
}
