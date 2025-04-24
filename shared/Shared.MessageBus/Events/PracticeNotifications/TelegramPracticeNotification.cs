namespace Shared.MessageBus.Events.PracticeNotifications;

public record TelegramPracticeNotification(string Email) : IBasePracticeNotification
{
    public Task AcceptAsync(IPracticeNotificationVisitor practiceNotificationVisitor)
    {
        return practiceNotificationVisitor.VisitAsync(this);
    }
}
