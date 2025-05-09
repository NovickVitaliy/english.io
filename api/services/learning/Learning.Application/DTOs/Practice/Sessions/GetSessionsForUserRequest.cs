namespace Learning.Application.DTOs.Practice.Sessions;

public record GetSessionsForUserRequest(
    string UserEmail,
    int PageNumber = 1,
    int PageSize = 10);
