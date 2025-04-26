using Fluxor;
using Learning.Features.Practice.Models.ReadingComprehension;
using Learning.Features.Practice.Services;
using Learning.Store.Practice;
using Learning.Store.PracticeStatus;
using Learning.Store.PracticeStatus.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Refit;
using Shared.Extensions;
using Shared.Store.User;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Learning.Features.Practice.Pages;

public partial class ReadingComprehension : ComponentBase
{
    [Inject] private IPracticeService PracticeService { get; init; } = null!;
    [Inject] private IStringLocalizer<ReadingComprehension> Localizer { get; init; } = null!;
    [Inject] private IState<PracticeStatusState> PracticeState { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private IState<FIllInTheGapsState> FillInTheGapsState { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private ISnackbar Snackbar { get; init; } = null!;
    private ReadingComprehensionExercise? _readingComprehensionExercise;
    private CheckReadingComprehensionExerciseRequest? _checkReadingComprehensionExerciseRequest;
    private CheckReadingComprehensionExerciseResult? _checkReadingComprehensionExerciseResult;
    private bool _overlayVisible = false;

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrWhiteSpace(UserState.Value.Token) && FillInTheGapsState.Value is not null)
        {
            _readingComprehensionExercise = await PracticeService.GetReadingComprehensionExercise(FillInTheGapsState.Value.Words, UserState.Value.Token);
            _checkReadingComprehensionExerciseRequest = new CheckReadingComprehensionExerciseRequest(_readingComprehensionExercise.Questions, _readingComprehensionExercise.Text);
        }
    }

    private async Task CheckReadingComprehension()
    {
        if (_checkReadingComprehensionExerciseRequest is null) return;

        _overlayVisible = true;
        try
        {
            _checkReadingComprehensionExerciseResult = await PracticeService.CheckReadingComprehensionExercise(_checkReadingComprehensionExerciseRequest, UserState.Value.Token);
            Dispatcher.Dispatch(new SetFourthTaskPercentageSuccessAction((double)_checkReadingComprehensionExerciseResult!.AnswersCorrect / _checkReadingComprehensionExerciseRequest.Questions.Length * 100));
        }
        catch (ApiException e)
        {
            var problemDetails = e.ToProblemDetails();
            Snackbar.Add(Localizer[problemDetails.Detail ?? "Error_Occured"], Severity.Error);
        }
        finally
        {
            _overlayVisible = false;
        }
    }

    private void NextExercise()
    {
        NavigationManager.NavigateTo("/practice/example-text");
    }
}

