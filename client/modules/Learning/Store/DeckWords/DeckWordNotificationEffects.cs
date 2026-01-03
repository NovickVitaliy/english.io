using Fluxor;
using Learning.Features.Decks.Components;
using Learning.Store.DeckWords.Actions;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.Store;

namespace Learning.Store.DeckWords;

public class DeckWordNotificationEffects : BaseEffects
{
    private readonly ISnackbar _snackbar;
    private readonly IStringLocalizer<AddDeckWordModal> _localizer;

    public DeckWordNotificationEffects(ISnackbar snackbar, IStringLocalizer<AddDeckWordModal> localizer)
    {
        _snackbar = snackbar;
        _localizer = localizer;
    }

    [EffectMethod]
    public Task HandleCreateDeckWordSuccessAction(CreateDeckWordSuccessAction action, IDispatcher dispatcher)
    {
        _snackbar.Add(_localizer["Api_Success"], Severity.Success);
        return Task.CompletedTask;
    }

    [EffectMethod]
    public Task HandleCreateDeckWordFailureAction(CreateDeckWordFailureAction action, IDispatcher dispatcher)
    {
        _snackbar.Add(action.ErrorMessage, Severity.Success);
        return Task.CompletedTask;
    }
}
