using Fluxor;
using Learning.Features.Practice.Services;
using Learning.Store.Practice.TranslateWordsTask.Get.Actions;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice.TranslateWordsTask.Get;

public class TranslateWordsTaskEffects : BaseEffects
{
    private readonly IPracticeService _practiceService;
    private readonly IState<UserState> _userState;

    public TranslateWordsTaskEffects(IPracticeService practiceService, IState<UserState> userState)
    {
        _practiceService = practiceService;
        _userState = userState;
    }

    [EffectMethod]
    public async Task HandleGetTranslatePracticeAction(GetTranslationTaskAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _practiceService.GetTranslationTaskAsync(action.DeckId, action.Request, _userState.Value.Token),
            response => new GetTranslationTaskSuccessAction(response),
            em => new GetTranslationTaskFailureAction(em),
            dispatcher);
    }
}
