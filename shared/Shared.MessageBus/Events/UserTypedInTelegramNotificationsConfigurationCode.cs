namespace Shared.MessageBus.Events;

public record UserTypedInTelegramNotificationsConfigurationCode(string Code, long ChatId) : BaseEvent;
