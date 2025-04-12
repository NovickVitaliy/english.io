namespace Shared.MessageBus.Events;

public record UserConfiguredTelegramNotifications(long ChatId, bool Success);
