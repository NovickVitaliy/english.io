using Learning.Features.Practice.Models.ReadingComprehension;
using Shared.Store.Markers;

namespace Learning.Store.Practice.ReadingComprehensionTask.Get.Actions;

public record GetReadingComprehensionTaskSuccessAction(ReadingComprehensionExercise ReadingComprehensionExercise) : IApiCompletedAction;