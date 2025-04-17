using Quartz;

namespace Learning.Infrastructure.Jobs;

public class NotificationJob : IJob
{
    public static string Name => nameof(NotificationJob);

    public Task Execute(IJobExecutionContext context)
    {
        var userEmail = context.MergedJobDataMap.GetString(JobConstants.UserEmailJobDataKey);
        var notificationChannel = context.MergedJobDataMap.GetString(JobConstants.NotificationChannelJobDataKey);
        return Task.CompletedTask;
    }
}
