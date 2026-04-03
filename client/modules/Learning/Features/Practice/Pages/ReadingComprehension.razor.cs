using Fluxor;
using Fluxor.Blazor.Web.Components;
using Learning.Features.Practice.Models.ReadingComprehension;
using Learning.Store.Practice;
using Learning.Store.Practice.ReadingComprehensionTask.Check;
using Learning.Store.Practice.ReadingComprehensionTask.Check.Actions;
using Learning.Store.Practice.ReadingComprehensionTask.Get;
using Learning.Store.Practice.ReadingComprehensionTask.Get.Actions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Learning.Features.Practice.Pages;

public partial class ReadingComprehension : FluxorComponent
{
    [Parameter] public Guid DeckId { get; init; }
    [Inject] private IStringLocalizer<ReadingComprehension> Localizer { get; init; } = null!;
    [Inject] private IDispatcher Dispatcher { get; init; } = null!;
    [Inject] private NavigationManager NavigationManager { get; init; } = null!;
    [Inject] private IState<ReadingComprehensionTaskState> ReadingComprehensionTaskState { get; init; } = null!;
    [Inject] private IState<PracticeState> PracticeState { get; init; } = null!;
    [Inject] private IState<ReadingComprehesionTaskResultState> ReadingComprehensionTaskResultState { get; init; } = null!;
    private CheckReadingComprehensionExerciseRequest? _checkReadingComprehensionExerciseRequest = null!;

    protected override void OnParametersSet()
    {
        Dispatcher.Dispatch(new GetReadingComprehesionTaskAction(PracticeState.Value.WordsForPractice));
        _checkReadingComprehensionExerciseRequest = new CheckReadingComprehensionExerciseRequest(new List<string>(), 10, string.Empty);
        ReadingComprehensionTaskState.StateChanged += (_, _) =>
        {
            if (ReadingComprehensionTaskState.Value.ReadingComprehensionExercise is not null)
            {
                _checkReadingComprehensionExerciseRequest.Text = ReadingComprehensionTaskState.Value.ReadingComprehensionExercise.Text;
                _checkReadingComprehensionExerciseRequest.Questions = new List<string>(ReadingComprehensionTaskState.Value.ReadingComprehensionExercise.Questions);
            }
        };
    }

    private void CheckReadingComprehension()
    {
        if (_checkReadingComprehensionExerciseRequest is not null)
        {
            Dispatcher.Dispatch(new CheckReadingComprehensionTaskAction(_checkReadingComprehensionExerciseRequest));
        }
    }

    private void NextExercise()
    {
        NavigationManager.NavigateTo("/practice/example-text");
    }
}
