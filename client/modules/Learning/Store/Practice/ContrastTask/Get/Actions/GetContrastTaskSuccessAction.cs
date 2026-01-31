using Learning.Features.Practice.Models.ContrastTask;
using Shared.Store.Markers;

namespace Learning.Store.Practice.ContrastTask.Get.Actions;

public record GetContrastTaskSuccessAction(ContrastTaskUnit[] ContrastTask) : IApiCompletedAction;