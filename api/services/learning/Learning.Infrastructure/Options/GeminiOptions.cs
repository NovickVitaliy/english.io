using System.ComponentModel.DataAnnotations;

namespace Learning.Infrastructure.Options;

public class GeminiOptions
{
    public const string ConfigurationKey = "GeminiOptions";

    [Required]
    public string ApiKey { get; init; } = null!;

    [Required]
    public string GenerateContentUrl { get; init; } = null!;
}
