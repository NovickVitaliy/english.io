using FluentValidation;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Notifications.API.DTOs;
using Notifications.API.Options;
using Shared.ErrorHandling;

namespace Notifications.API.Services;

public class MailKitEmailService : IEmailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<MailKitEmailService> _logger;
    private readonly IEnumerable<IValidator> _validators;

    public MailKitEmailService(IOptions<MailSettings> mailSettingsOptions, ILogger<MailKitEmailService> logger, IEnumerable<IValidator> validators)
    {
        _logger = logger;
        _validators = validators;
        _mailSettings = mailSettingsOptions.Value;
    }

    public async Task<Result<bool>> SendMessageAsync(SendEmailMessageRequest request)
    {
        try
        {
            var errorMessage = ValidateRequest(request);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                return Result<bool>.BadRequest(errorMessage);
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

    private string? ValidateRequest(SendEmailMessageRequest request)
    {
        return _validators.OfType<IValidator<SendEmailMessageRequest>>()
            .Select(x => x.Validate(request))
            .Where(x => x is { Errors.Count: > 0, IsValid: false })
            .SelectMany(x => x.Errors)
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
    }
}
