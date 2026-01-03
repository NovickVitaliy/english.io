using Learning.Features.Settings.Models.Sessions;
using Shared.Store.Markers;

namespace Learning.Store.Sessions.Actions;

public record FetchSessionsResultAction(SessionDto[] Sessions, long Count) : IApiCompletedAction;
