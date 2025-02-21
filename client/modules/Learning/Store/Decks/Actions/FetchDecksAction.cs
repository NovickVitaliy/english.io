using Learning.Features.Decks.Models;

namespace Learning.Store.Decks.Actions;

public record FetchDecksAction(string UserEmail, int PageNumber, int PageSize);
