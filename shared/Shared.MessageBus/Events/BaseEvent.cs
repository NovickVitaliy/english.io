namespace Shared.MessageBus.Events;

public abstract record BaseEvent
{
    public DateTime CreatedOn { get; } = DateTime.UtcNow;
}