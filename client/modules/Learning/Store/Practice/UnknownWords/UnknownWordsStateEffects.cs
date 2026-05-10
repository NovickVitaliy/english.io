using Fluxor;
using Learning.Features.Decks.Models;
using Learning.Features.Practice.Pages;
using Learning.Features.Practice.Services;
using Learning.LearningShared.Services;
using Learning.Store.Practice.Actions;
using Learning.Store.Practice.UnknownWords.Actions;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.Practice.UnknownWords;

public class UnknownWordsStateEffects : BaseEffects
{
    private readonly IState<UserState> _userState;
    private readonly IDecksService _decksService;
    private readonly ISnackbar _snackbar;
    private readonly IStringLocalizer<PracticeResult> _localizer;

    public UnknownWordsStateEffects(IState<UserState> userState, IDecksService decksService, ISnackbar snackbar, IStringLocalizer<PracticeResult> localizer)
    {
        _userState = userState;
        _decksService = decksService;
        _snackbar = snackbar;
        _localizer = localizer;
    }

    [EffectMethod]
    public async Task HandleSaveUnknownWordAction(SaveUnknownWord action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _decksService.CreateDeckWordAsync(action.DeckId, new CreateDeckWordRequest()
            {
                Word = action.Word
            }, _userState.Value.Token),
            _ => new SaveUnknownWordSuccessAction(action.Word),
            _ => new SaveUnknownWordFailureAction(),
            dispatcher);
    }

    [EffectMethod]
    public async Task HandleSaveUnknownWordFailureAction(SaveUnknownWordFailureAction action, IDispatcher dispatcher)
    {
        _snackbar.Add(_localizer["Word_Already_Exists_In_Deck"], Severity.Error);
    }

    [EffectMethod]
    public async Task HandleSaveUnknownWordSuccessAction(SaveUnknownWordSuccessAction action, IDispatcher dispatcher)
    {
        _snackbar.Add(_localizer["Saved"], Severity.Success);
    }
}
