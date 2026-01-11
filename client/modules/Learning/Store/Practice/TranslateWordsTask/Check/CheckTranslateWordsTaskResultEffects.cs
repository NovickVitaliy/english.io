using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.TranslateWordsTask.Check.Actions;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice.TranslateWordsTask.Check;

public class CheckTranslateWordsTaskResultEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public CheckTranslateWordsTaskResultEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleCheckTranslateWordsTaskAction(CheckTranslateWordsTaskAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _practiceService.CheckTranslateWordsTaskAsync(action.Request, _userState.Value.Token),
            response => new CheckTranslateWordsTaskSuccessAction(response.Results),
            errorMessage => new CheckTranslateWordsTaskFailureAction(errorMessage),
            dispatcher);
    }
}
