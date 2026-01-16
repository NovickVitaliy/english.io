using Learning.Features.Practice.Models.GetWordsForPractice;
using Shared.Store.Markers;

namespace Learning.Store.Practice.FillInTheGapsTask.Get.Actions;

public record GetFillInTheGapsTaskAction(Guid DeckId, WordForPractice[] WordsForPractice) : IApiAction;
