using MimeKit.Text;

namespace Notifications.API.DTOs;

public record SendEmailMessageRequest(
    string ReceiverEmail,
    string ReceiverName,
    string Subject,
    TextFormat? TextFormat,
    string Body);
