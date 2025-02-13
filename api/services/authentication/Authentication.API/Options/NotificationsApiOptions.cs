using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Options;

public class NotificationsApiOptions
{
    public const string ConfigurationKey = "NotificationsApi";

    [Required]
    public string Http { get; init; } = null!;

    [Required]
    public string Https { get; init; } = null!;

    [Required]
    public bool IsHttps { get; init; }

    [Required]
    public string Key { get; init; } = null!;
}
