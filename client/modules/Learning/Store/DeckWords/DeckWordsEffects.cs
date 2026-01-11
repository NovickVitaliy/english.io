using Fluxor;
using Learning.Features.Decks.Components;
using Learning.LearningShared.Services;
using Learning.Store.DeckWords.Actions.Create;
using Learning.Store.DeckWords.Actions.Delete;
using Microsoft.Extensions.Localization;
using Shared.Store;
using Shared.Store.User;

namespace Learning.Store.DeckWords;

public class DeckWordsEffects : BaseEffects
{
    private readonly IDecksService _decksService;
    private readonly IState<UserState> _state;
    private readonly IStringLocalizer<AddDeckWordModal> _localizer;

    public DeckWordsEffects(IDecksService decksService, IState<UserState> state, IStringLocalizer<AddDeckWordModal> localizer)
    {
        _decksService = decksService;
        _state = state;
        _localizer = localizer;
    }

    [EffectMethod]
    public async Task HandleCreateDeckWordAction(CreateDeckWordAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequest(
            async () => await _decksService.CreateDeckWordAsync(action.DeckId, action.Request, _state.Value.Token),
            response => new CreateDeckWordSuccessAction(response),
            errorMessage => new CreateDeckWordFailureAction(errorMessage),
            dispatcher);
    }

    [EffectMethod]
    public async Task HandleDeleteDeckEntryAction(DeleteDeckEntryAction action, IDispatcher dispatcher)
    {
        await ProcessRefitApiRequestWithNoResponse(
            async () => await _decksService.DeleteDeckEntryAsync(action.DeckId, action.WordEntryId, _state.Value.Token),
            () => new DeleteDeckEntrySuccessAction(action.WordEntryId),
            em => new DeleteDeckEntryFailureAction(em),
            dispatcher);
    }
}
