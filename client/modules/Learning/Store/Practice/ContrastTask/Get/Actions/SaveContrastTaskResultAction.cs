using Learning.Features.Practice.Models.ContrastTask;
using Shared.Store.Markers;

namespace Learning.Store.Practice.ContrastTask.Get.Actions;

public record SaveContrastTaskResultAction(SaveContrastTaskResultRequest Request) : IApiAction;

public record SaveContrastTaskResultSuccessAction : IApiCompletedAction;

public record SaveContrastTaskResultFailureAction(string ErrorMessage);
