using Learning.Features.Practice.Models.FillInTheGaps;
using Shared.Store.Markers;

namespace Learning.Store.Practice.FillInTheGapsTask.Get.Actions;

public record GetFillInTheGapsTaskSuccessAction(SentenceWithGap[] SentencesWithGap) : IApiCompletedAction;
