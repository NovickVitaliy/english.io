namespace Shared.MessageBus.Events.PracticeNotifications;

public interface IPracticeNotificationVisitor
{
    public Task VisitAsync(EmailPracticeNotification practiceNotification);
    public Task VisitAsync(TelegramPracticeNotification practiceNotification);
}
