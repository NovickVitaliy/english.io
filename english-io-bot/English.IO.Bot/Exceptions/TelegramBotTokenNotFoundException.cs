namespace English.IO.Bot.Exceptions;

public class TelegramBotTokenNotFoundException : Exception
{
    public TelegramBotTokenNotFoundException()
        : base("Telegram Bot token was not found in the app configurations")
    { }
}