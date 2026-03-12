using Learning.Features.Practice.Models.ReadingComprehension;
using Shared.Store.Markers;

namespace Learning.Store.Practice.ReadingComprehensionTask.Check.Actions;

public record CheckReadingComprehensionTaskSuccessAction(CheckReadingComprehensionExerciseResult Result) : IApiCompletedAction;
