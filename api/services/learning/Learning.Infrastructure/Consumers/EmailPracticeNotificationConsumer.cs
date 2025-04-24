using System.Text.Json;
using Authentication.API.DTOs.Other;
using MassTransit;
using Shared.MessageBus.Events.PracticeNotifications;
using Shared.Services;

namespace Learning.Infrastructure.Consumers;

public class EmailPracticeNotificationConsumer : IConsumer<EmailPracticeNotification>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EmailPracticeNotificationConsumer(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task Consume(ConsumeContext<EmailPracticeNotification> context)
    {
        var email = context.Message.Email;
        var client = _httpClientFactory.CreateClient(SharedServicesConstants.NotificationHttpClientName);

        var sendEmailMessageRequest = new SendEmailMessageRequest(
                context.Message.Email,
                context.Message.Email,
                "Practice Notification",
                "text/html",
                "<h1>Hi! Its time to practice english</h1>");

        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"api/notifications/send-message", UriKind.Relative),
            Content = new StringContent(JsonSerializer.Serialize(sendEmailMessageRequest))
        };

        await client.SendAsync(request);
    }
}
