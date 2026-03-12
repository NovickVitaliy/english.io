using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Practice.Models.FillInTheGaps;
using Learning.Store.Practice;
using Learning.Store.Practice.FillInTheGapsTask.Check;
using Learning.Store.Practice.FillInTheGapsTask.Check.Actions;
using Learning.Store.Practice.FillInTheGapsTask.Get;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Learning.Features.Practice.Pages;

public partial class FillInTheGapsPage : FluxorComponent
{
    [Parameter] public Guid DeckId { get; init; }
    [Inject] private IStringLocalizer<FillInTheGapsPage> Localizer { get; init; } = null!;
    [Inject] private IState<FillInTheGapsTaskState> FillInTheGapsState { get; init; } = null!;
    [Inject] private IState<PracticeState> PracticeState { get; init; } = null!;
    [Inject] private IState<FillInTheGapsTaskResultState> FillInTheGapsTaskResultState { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    private CheckFillInTheGapsTaskRequest _request = null!;

    protected override Task OnParametersSetAsync()
    {
        // Dispatcher.Dispatch(new GetFillInTheGapsTaskAction(DeckId, PracticeState.Value.WordsForPractice));
        _request = new CheckFillInTheGapsTaskRequest(DeckId, PracticeState.Value.WordsForPractice.Length);
        base.OnInitialized();
        return Task.CompletedTask;
    }

    private void VerifySentences()
    {
        Dispatcher.Dispatch(new CheckFillInTheGapsTaskAction(_request));
    }

    private void NextExercise()
    {
        NavigationManager.NavigateTo($"/practice/{DeckId}/contrast-task");
    }

    private bool HasVerified => !FillInTheGapsTaskResultState.Value.IsLoading && FillInTheGapsTaskResultState.Value.SentencesWithFilledGapsResults.Length > 0;
}

