using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.ContrastTask.Get.Actions;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice.ContrastTask.Get;

public class ContrastTaskEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public ContrastTaskEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleGetContrastTaskAction(GetContrastTaskAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _practiceService.GetContrastTaskAsync(action.DeckId, action.WordsForPractice, _userState.Value.Token),
            response => new GetContrastTaskSuccessAction(response),
            em => new GetContrastTaskFailureAction(em),
            dispatcher);
    }

    [EffectMethod]
    public async Task HandleSaveContrastTaskResultAction(SaveContrastTaskResultAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequestWithNoResponse(
            async () => await _practiceService.SaveContrastTaskResultAsync(action.Request, _userState.Value.Token),
            () => new SaveContrastTaskResultSuccessAction(),
            em => new SaveContrastTaskResultFailureAction(em),
            dispatcher);
    }
}
