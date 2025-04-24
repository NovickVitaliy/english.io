using Learning.Domain.Models;
using Quartz;
using Shared.MessageBus.Events.PracticeNotifications;

namespace Learning.Infrastructure.Jobs;

public class NotificationJob : IJob
{
    public static string Name => nameof(NotificationJob);

    private readonly IPracticeNotificationVisitor _practiceNotificationVisitor;

    public NotificationJob(IPracticeNotificationVisitor practiceNotificationVisitor)
    {
        _practiceNotificationVisitor = practiceNotificationVisitor;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var userEmail = context.MergedJobDataMap.GetString(JobConstants.UserEmailJobDataKey);
        var notificationChannel = context.MergedJobDataMap.GetString(JobConstants.NotificationChannelJobDataKey);

        ArgumentNullException.ThrowIfNull(userEmail);
        ArgumentNullException.ThrowIfNull(notificationChannel);

        var notification = GetNotificationEvent(notificationChannel, userEmail);

        await notification.AcceptAsync(_practiceNotificationVisitor);
    }

    private static IBasePracticeNotification GetNotificationEvent(string notificationChannel, string userEmail) =>
        Enum.Parse<NotificationChannel>(notificationChannel) switch
        {
            NotificationChannel.Email => new EmailPracticeNotification(userEmail),
            NotificationChannel.Telegram => new TelegramPracticeNotification(userEmail),
            _ => throw new ArgumentOutOfRangeException(nameof(notificationChannel), notificationChannel, null)
        };
}
