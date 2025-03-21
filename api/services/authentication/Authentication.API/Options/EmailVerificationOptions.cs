using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Options;

public class EmailVerificationOptions
{
    public const string ConfigurationKey = "EmailVerificationOptions";

    [Required]
    public IReadOnlyDictionary<string, string> MessageTemplatesByLanguage { get; init; } = null!;

    [Required]
    public IReadOnlyDictionary<string, string> MessageHeadersByLanguage { get; init; } = null!;
}
