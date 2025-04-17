namespace Shared.MessageBus.Events;

public record UserConfiguredTelegramNotifications(long ChatId, string UserEmail, bool Success);
