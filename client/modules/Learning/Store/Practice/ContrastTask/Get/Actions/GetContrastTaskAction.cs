using Learning.Features.Practice.Models.GetWordsForPractice;
using Shared.Store.Markers;

namespace Learning.Store.Practice.ContrastTask.Get.Actions;

public record GetContrastTaskAction(Guid DeckId, WordForPractice[] WordsForPractice) : IApiAction;
