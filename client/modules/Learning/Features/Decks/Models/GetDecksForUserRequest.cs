namespace Learning.Features.Decks.Models;

public record GetDecksForUserRequest(
    string Email,
    int PageNumber = 1,
    int PageSize = 10);
