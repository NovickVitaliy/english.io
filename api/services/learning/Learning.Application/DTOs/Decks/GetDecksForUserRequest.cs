namespace Learning.Application.DTOs.Decks;

public record GetDecksForUserRequest(
    string Email,
    int PageNumber = 1,
    int PageSize = 10);
