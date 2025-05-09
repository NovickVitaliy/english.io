namespace Learning.Features.Settings.Models.Sessions;

public record GetSessionResultsForUserRequest(int PageNumber, int PageSize, string UserEmail);
