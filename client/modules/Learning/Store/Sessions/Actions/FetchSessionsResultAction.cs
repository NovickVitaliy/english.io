using Learning.Features.Settings.Models.Sessions;

namespace Learning.Store.Sessions.Actions;

public record FetchSessionsResultAction(SessionDto[] Sessions, long Count);
