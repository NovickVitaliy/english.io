using Learning.Features.Practice.Models.TranslateWordsTask.Get;
using Shared.Store.Markers;

namespace Learning.Store.Practice.TranslateWordsTask.Get.Actions;

public record GetTranslationTaskAction(Guid DeckId, GetTranlationTaskRequest Request) : IApiAction;
