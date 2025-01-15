namespace Shared.MessageBus.Events;

public record UserCreatedPreferences(string UserEmail) : BaseEvent;