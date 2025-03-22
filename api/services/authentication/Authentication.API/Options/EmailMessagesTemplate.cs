using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Options;

public class EmailMessagesTemplate
{
    public const string EmailVerificationOptionsKey = "EmailVerificationOptions";
    public const string ForgotPasswordOptionsKey = "ForgotPasswordOptions";

    [Required]
    public IReadOnlyDictionary<string, string> MessageTemplatesByLanguage { get; init; } = null!;

    [Required]
    public IReadOnlyDictionary<string, string> MessageHeadersByLanguage { get; init; } = null!;

}
