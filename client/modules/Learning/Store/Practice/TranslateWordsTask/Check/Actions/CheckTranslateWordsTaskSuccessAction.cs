using Learning.Features.Practice.Models.TranslateWordsTask.Check;
using Shared.Store.Markers;

namespace Learning.Store.Practice.TranslateWordsTask.Check.Actions;

public record CheckTranslateWordsTaskSuccessAction(TranslateWordResult[] Results) : IApiCompletedAction;
