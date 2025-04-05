using Fluxor;
using Learning.Features.Practice.Models.FillInTheGaps;
using Learning.Features.Practice.Services;
using Learning.Store.Practice;
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
    private SentenceWithGap[]? _sentencesWithGaps;
    private FillInTheGapsData? _fillInTheGapsData;
    private bool _hasVerified;

    protected override async Task OnParametersSetAsync()
    {
        if (FillInTheGapsState.Value is not null && UserState.Value is not null)
        {
            _sentencesWithGaps = await PracticeService.GenerateSentencesWithGaps(FillInTheGapsState.Value.Words, UserState.Value.Token);
            _fillInTheGapsData = new FillInTheGapsData(FillInTheGapsState.Value.Words.Length);
        }
    }

    private void VerifySentences()
    {
        _hasVerified = true;
    }

    private void NextExercise()
    {
        NavigationManager.NavigateTo("/practice/example-text");
    }
}

