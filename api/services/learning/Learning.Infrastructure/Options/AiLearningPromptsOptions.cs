using System.ComponentModel.DataAnnotations;
using MassTransit.Futures.Contracts;

namespace Learning.Infrastructure.Options;

public class AiLearningPromptsOptions
{
    public const string ConfigurationKey = "AiLearningPrompts";

    [Required]
    public string PromptForWordTranslatingWithExampleSentences { get; init; } = null!;

    [Required]
    public string PromptForCheckingIfWordCompliesToTheTopic { get; init; } = null!;

    [Required]
    public string PromptForCheckingIfTranslationsAreCorrect { get; init; } = null!;
}
