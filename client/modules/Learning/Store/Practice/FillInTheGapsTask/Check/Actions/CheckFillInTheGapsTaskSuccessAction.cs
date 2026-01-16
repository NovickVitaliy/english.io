using Learning.Features.Practice.Models.FillInTheGaps;
using Shared.Store.Markers;

namespace Learning.Store.Practice.FillInTheGapsTask.Check.Actions;

public record CheckFillInTheGapsTaskSuccessAction(SentenceWithFilledGapResult[] SentenceWithFilledGapResults) : IApiCompletedAction;