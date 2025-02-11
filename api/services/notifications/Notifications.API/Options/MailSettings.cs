using System.ComponentModel.DataAnnotations;

namespace Notifications.API.Options;

public class MailSettings
{
    public const string ConfigurationKey = "MailSettings";

    [Required]
    public string Server { get; init; } = null!;

    [Required]
    public int Port { get; init; }

    [Required]
    public string SenderName { get; init; } = null!;

    [Required]
    public string SenderEmail { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;

    [Required]
    public string Username { get; init; } = null!;
}
