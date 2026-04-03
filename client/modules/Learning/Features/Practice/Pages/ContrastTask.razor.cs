using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Practice.Models.ContrastTask;
using Learning.Store.Practice;
using Learning.Store.Practice.ContrastTask.Get;
using Learning.Store.Practice.ContrastTask.Get.Actions;
using Learning.Store.Practice.UnknownWords;
using Learning.Store.Practice.UnknownWords.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;

namespace Learning.Features.Practice.Pages;

public partial class ContrastTask : FluxorComponent
{
    [Parameter] public Guid DeckId { get; init; }
    [Inject] private IState<PracticeState> PracticeState { get; init; } = null!;
    [Inject] private IState<ContrastTaskState> ContrastTaskState { get; init; } = null!;
    [Inject] private IState<UnknownWordsState> UnknownWordsState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private IStringLocalizer<ContrastTask> Localizer { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IJSRuntime? JsRuntime { get; init; }
    private bool HasVerified { get; set; } = false;
    private SaveContrastTaskResultRequest _request = null!;

    protected override void OnParametersSet()
    {
        // Dispatcher.Dispatch(new GetContrastTaskAction(DeckId, PracticeState.Value.WordsForPractice));
        _request = new SaveContrastTaskResultRequest(DeckId, PracticeState.Value.WordsForPractice);
    }

    private void AddUnknownWord(string word)
    {
        if (!UnknownWordsState.Value.UnknownWords.Contains(word))
        {
            Dispatcher.Dispatch(new AddUnknownWordAction(word));
            Snackbar.Add(Localizer["Unknown_Word_Added"], Severity.Info);
        }
        else
        {
            Snackbar.Add(Localizer["Unknown_Word_Already_Exists"], Severity.Info);
        }
    }

    private void NextExercise()
    {
        NavigationManager.NavigateTo($"/practice/{DeckId}/reading-comprehension");
    }

    private void SetChosenWord(Guid senseId, string word)
    {
        _request.AnswersMap[senseId] = (word == ContrastTaskState.Value.ForSense(senseId), word);
    }

    private async Task VerifyTask()
    {
        HasVerified = true;
        Dispatcher.Dispatch(new SaveContrastTaskResultAction(_request));
        await JsRuntime.InvokeVoidAsync("scrollToTop");
    }
}

