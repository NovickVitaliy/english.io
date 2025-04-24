using MassTransit;

namespace Shared.MessageBus.Events.PracticeNotifications;

public interface IBasePracticeNotification
{
    string Email { get; init; }
    public abstract Task AcceptAsync(IPracticeNotificationVisitor practiceNotificationVisitor);
}
