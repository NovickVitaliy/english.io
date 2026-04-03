using Learning.Features.Practice.Models.GetWordsForPractice;
using Shared.Store.Markers;

namespace Learning.Store.Practice.ReadingComprehensionTask.Get.Actions;

public record GetReadingComprehesionTaskAction(WordForPractice[] WordsForPractice) : IApiAction;
