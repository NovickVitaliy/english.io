using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Notifications.API.DTOs;
using Notifications.API.Options;
using Shared.ErrorHandling;

namespace Notifications.API.Services.Email;

public class MailKitEmailService : IEmailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<MailKitEmailService> _logger;

    public MailKitEmailService(IOptions<MailSettings> mailSettingsOptions, ILogger<MailKitEmailService> logger)
    {
        _logger = logger;
        _mailSettings = mailSettingsOptions.Value;
    }

    public async Task<Result<bool>> SendMessageAsync(SendEmailMessageRequest request)
    {
        try
        {
            var validationResult = request.IsValid();
            if (!validationResult.IsValid)
            {
                return Result<bool>.BadRequest(validationResult.ErrorMessage);
            }

            using var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            msg.To.Add(new MailboxAddress(request.ReceiverName, request.ReceiverEmail));
            msg.Subject = request.Subject;
            msg.Body = new TextPart(request.TextFormat!.Value)
            {
                Text = request.Body
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.Password);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
            return Result<bool>.Ok(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured");
            return Result<bool>.BadRequest(e.Message);
        }
    }
}
