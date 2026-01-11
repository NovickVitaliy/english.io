using Learning.Features.Practice.Models.GetWordsForPractice;
using Shared.Store.Markers;

namespace Learning.Store.Practice.Actions;

public record GetWordsForPracticeSuccessAction(GetWordsForPracticeResponse Response) : IApiCompletedAction;
