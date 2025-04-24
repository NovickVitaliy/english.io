using MassTransit;

namespace Shared.MessageBus.Events.PracticeNotifications;

public class PracticeNotificationVisitor : IPracticeNotificationVisitor
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PracticeNotificationVisitor(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task VisitAsync(EmailPracticeNotification practiceNotification)
    {
        return _publishEndpoint.Publish(practiceNotification);
    }
    public Task VisitAsync(TelegramPracticeNotification practiceNotification)
    {
        return _publishEndpoint.Publish(practiceNotification);
    }
}
