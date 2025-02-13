using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTOs;
using Notifications.API.Services;
using Notifications.API.Services.Email;

namespace Notifications.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize(AuthenticationSchemes = "ApiKey")]
public class NotificationsController : ControllerBase
{
    private readonly IEmailService _emailService;

    public NotificationsController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendEmailMessage(SendEmailMessageRequest request)
    {
        return (await _emailService.SendMessageAsync(request)).ToApiResponse();
    }
}
