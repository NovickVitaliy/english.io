namespace Learning.Store.Sessions.Actions;

public record FetchSessionsAction(string UserEmail, int CurrentPage, int PageSize);
