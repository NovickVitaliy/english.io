namespace Learning.Features.Settings.Models.Sessions;

public record GetSessionsResultsForUserResponse(
    SessionDto[] Sessions,
    long Count);
