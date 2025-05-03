namespace Learning.Application.DTOs.Practice.Sessions;

public record GetPracticeSessionsForUserResponse(
    SessionDto[] Sessions,
    long Count
    );
