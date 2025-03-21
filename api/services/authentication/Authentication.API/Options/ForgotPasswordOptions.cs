using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Options;

public class ForgotPasswordOptions
{
    public const string ConfigurationKey = "ForgotPasswordOptions";

    [Required]
    public IReadOnlyDictionary<string, string> MessageTemplatesByLanguage { get; init; } = null!;

    [Required]
    public IReadOnlyDictionary<string, string> MessageHeadersByLanguage { get; init; } = null!;
}
