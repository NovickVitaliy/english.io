using System.ComponentModel.DataAnnotations;
using MassTransit.Futures.Contracts;

namespace Learning.Infrastructure.Options;

public class AiLearningPromptsOptions
{
    public const string ConfigurationKey = "AiLearningPrompts";

    [Required]
    public string PromptForWordTranslatingWithExampleSentences { get; init; } = null!;
}
