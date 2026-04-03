using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.ExampleText.Get.Actions;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice.ExampleText.Get;

public class ExampleTextTaskEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public ExampleTextTaskEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleGetExampleTextTaskAction(GetExampleTextTaskAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            () => _practiceService.GenerateExampleTextAsync(action.WordsForPractice.Select(x => x.Word).ToArray(), _userState.Value.Token),
            response => new GetExampleTextTaskSuccessAction(response.Text),
            em => new GetExampleTextTaskFailureAction(em),
            dispatcher);
    }
}
