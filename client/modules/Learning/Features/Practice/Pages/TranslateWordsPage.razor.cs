using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Practice.Models.TranslateWordsTask.Check;
using Learning.Features.Practice.Models.TranslateWordsTask.Get;
using Learning.Features.Practice.Services;
using Learning.Store.Practice;
using Learning.Store.Practice.Actions;
using Learning.Store.Practice.TranslateWordsTask.Check;
using Learning.Store.Practice.TranslateWordsTask.Check.Actions;
using Learning.Store.Practice.TranslateWordsTask.Get;
using Learning.Store.Practice.TranslateWordsTask.Get.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared;
using Shared.Store.User;

namespace Learning.Features.Practice.Pages;

public partial class TranslateWordsPage : FluxorComponent
{
    [Parameter] public Guid DeckId { get; init; }
    [Inject] private IStringLocalizer<TranslateWordsPage> Localizer { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IPracticeService PracticeService { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private IState<PracticeState> PracticeState { get; init; } = null!;
    [Inject] private IState<TranslateWordsTaskState> TranslateWordsState { get; init; } = null!;
    [Inject] private IState<TranslateWordsTaskResultState> TranslateWordsTaskResultState { get; init; } = null!;
    [SupplyParameterFromQuery] private string OriginalLanguage { get; init; } = null!;
    [SupplyParameterFromQuery] private string TranslateLanguage { get; init; } = null!;
    private CheckTranslateWordsTaskRequest _taskRequest = null!;

    protected override void OnInitialized()
    {
        UserState.StateChanged += (_, _) => GetWordsForPracticeAsync();
        PracticeState.StateChanged += (_, _) => GetTranslationTaskAsync();
        base.OnInitialized();
    }

    private void GetTranslationTaskAsync()
    {
        _taskRequest = new CheckTranslateWordsTaskRequest(PracticeState.Value.WordsForPractice.Length, OriginalLanguage, TranslateLanguage, DeckId);
        Dispatcher.Dispatch(new GetTranslationTaskAction(DeckId, new GetTranlationTaskRequest(OriginalLanguage, TranslateLanguage, PracticeState.Value.WordsForPractice)));
    }

    private void GetWordsForPracticeAsync()
    {
        if (PracticeState.Value.WordsForPractice.Length == 0)
        {
            Dispatcher.Dispatch(new GetWordsForPracticeAction(DeckId));
        }
        else
        {
            GetTranslationTaskAsync();
        }
    }

    protected override void OnParametersSet()
    {
        if (!(GlobalConstants.Languages.SupportedLanguages.Contains(OriginalLanguage)
            && GlobalConstants.Languages.SupportedLanguages.Contains(TranslateLanguage)))
        {
            NavigationManager.NavigateTo("/learning/decks");
            return;
        }

        GetWordsForPracticeAsync();
    }

    private void VerifyTranslatedWords()
    {
        if (TranslateWordsTaskResultState.Value.Results.Length > 0)
        {
            Snackbar.Add(Localizer["Already_Verified"], Severity.Info);
        }

        Dispatcher.Dispatch(new CheckTranslateWordsTaskAction(_taskRequest));
    }

    private void NextExercise()
    {
        if (OriginalLanguage == "ukrainian")
        {
            Dispatcher.Dispatch(new ResetTranslateWordsTaskAction());
            NavigationManager.NavigateTo("/practice/fill-in-the-gaps");
            return;
        }

        Dispatcher.Dispatch(new ResetTranslateWordsTaskAction());
        NavigationManager.NavigateTo($"/practice/{DeckId}/translate-words?originalLanguage=ukrainian&translateLanguage=english");
    }
}

