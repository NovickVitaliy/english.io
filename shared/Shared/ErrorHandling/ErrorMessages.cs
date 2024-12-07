namespace Shared.ErrorHandling;
//TODO: change class to use localization         
public static class ErrorMessages
{
    public static string NoError()
    {
        return string.Empty;
    }

    public static string NotFound<TEntityType>(object key)
    {
        return $"Entity of type '{typeof(TEntityType).Name}' with key: '{key}' was not found.";
    }

    public static string BadRequest(string reason)
    {
        return $"Bad request. Reason: '{reason}'";
    }
}