using Fluxor;
using Learning.Features.Practice.Models.FillInTheGaps;
using Learning.Features.Practice.Services;
using Learning.Store.Practice;
using Learning.Store.PracticeStatus.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Shared.Store.User;

namespace Learning.Features.Practice.Pages;

public partial class FillInTheGapsPage : ComponentBase
{
    [Inject] private IStringLocalizer<FillInTheGapsPage> Localizer { get; init; } = null!;
    [Inject] private IPracticeService PracticeService { get; init; } = null!;
    [Inject] private IState<UserState> UserState { get; init; } = null!;
    [Inject] private IState<FIllInTheGapsState> FillInTheGapsState { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    private SentenceWithGap[]? _sentencesWithGaps;
    private FillInTheGapsData? _fillInTheGapsData;
    private bool _hasVerified;

    protected override async Task OnParametersSetAsync()
    {
        if (FillInTheGapsState.Value is not null && UserState.Value is not null)
        {
            _sentencesWithGaps = await PracticeService.GenerateSentencesWithGaps(FillInTheGapsState.Value.Words, UserState.Value.Token);
            _fillInTheGapsData = new FillInTheGapsData(FillInTheGapsState.Value.Words.Length, FillInTheGapsState.Value.Words);
        }
    }

    private void VerifySentences()
    {
        _hasVerified = true;
    }

    private void NextExercise()
    {
        Dispatcher.Dispatch(new SetThirdTaskPercentageSuccessAction((double)_fillInTheGapsData!.FillInTheGapsResult.Count(x => x.Key == x.Value) / _fillInTheGapsData.Words.Length * 100));
        NavigationManager.NavigateTo("/practice/example-text");
    }
}

