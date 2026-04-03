using Learning.Features.Practice.Models.GetWordsForPractice;
using Shared.Store.Markers;

namespace Learning.Store.Practice.ExampleText.Get.Actions;

public record GetExampleTextTaskAction(WordForPractice[] WordsForPractice) : IApiAction;
