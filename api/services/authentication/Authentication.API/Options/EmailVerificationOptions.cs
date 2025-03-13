using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Options;

public class EmailVerificationOptions
{
    public const string ConfigurationKey = "EmailVerificationOptions";

    [Required]
    public string MessageTemplate { get; init; } = null!;
}
