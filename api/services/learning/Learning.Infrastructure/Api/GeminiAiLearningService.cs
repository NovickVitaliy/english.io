using System.Text.Json;
using Google.GenAI;
using Google.GenAI.Types;
using Learning.Application.Contracts.Api;
using Learning.Application.DTOs.Decks;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.ReadingComprehension.Check;
using Learning.Application.DTOs.Practice.ReadingComprehension.Create;
using Learning.Application.DTOs.Practice.TranslateWords;
using Learning.Domain.Models;
using Learning.Infrastructure.Options;
using Microsoft.Extensions.Options;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Learning.Infrastructure.Api;

public class GeminiAiLearningService : IAiLearningService
{
    private readonly Client _client;
    private readonly GeminiOptions _geminiOptions;
    private readonly AiLearningPromptsOptions _aiLearningPromptsOptions;
    private readonly GenerateContentConfig _defaultGenerateContentConfig;

    public GeminiAiLearningService(
        Client client,
        IOptions<GeminiOptions> geminiOptions,
        IOptions<AiLearningPromptsOptions> aiLearningPromptsOptions)
    {
        _client = client;
        _geminiOptions = geminiOptions.Value;
        _aiLearningPromptsOptions = aiLearningPromptsOptions.Value;
        _defaultGenerateContentConfig = new GenerateContentConfig
        {
            ResponseMimeType = "application/json"
        };
    }

    public async Task<WordUnit?> GetTranslatedWordWithExamplesAsync(string word, int exampleSentences)
    {
        var prompt = _aiLearningPromptsOptions.PromptForWordTranslatingWithExampleSentences
            .Replace("{WORD}", word, StringComparison.InvariantCulture)
            .Replace("{EXAMPLES_COUNT}", exampleSentences.ToString(), StringComparison.InvariantCulture);

        return await GenerateInternal<WordUnit>(prompt);
    }

    public async Task<bool> DoesWordComplyToTheArticle(string word, string topic)
    {
        var prompt = _aiLearningPromptsOptions.PromptForCheckingIfWordCompliesToTheTopic
            .Replace("{word}", word, StringComparison.InvariantCulture)
            .Replace("{topic}", topic, StringComparison.InvariantCulture);

        return (await GenerateInternal<DoesWordComplyToTheTopicResponse>(prompt))!.DoesComply;
    }

    public async Task<TranslatedWordResult[]?> VerifyWordsTranslations(TranslateWordsRequest request)
    {
        var prompt = _aiLearningPromptsOptions.PromptForCheckingIfTranslationsAreCorrect
            .Replace("{words}", string.Join("; ", request.TranslatedWords.Select(x => x.OriginalWord)), StringComparison.InvariantCulture)
            .Replace("{originalLanguage}", request.OriginalLanguage, StringComparison.InvariantCulture)
            .Replace("{translatedWords}", string.Join("; ", request.TranslatedWords.Select(x => x.Translated)), StringComparison.InvariantCulture)
            .Replace("{translatedLanguage}", request.TranslatedLanguage, StringComparison.InvariantCulture);

        return (await GenerateInternal<TranslatedWordResult[]>(prompt));
    }

    public async Task<SentenceWithGap[]?> GenerateSentencesWithGaps(string[] words)
    {
        var prompt = _aiLearningPromptsOptions.PromptForGeneratingSentencesWithGaps
            .Replace("{words}", string.Join(',', words), StringComparison.InvariantCulture);

        return await GenerateInternal<SentenceWithGap[]>(prompt);
    }

    public async Task<string> GenerateExampleTextAsync(string[] words)
    {
        var prompt = _aiLearningPromptsOptions.PromptForGeneratingExampleText
            .Replace("{words}", string.Join(',', words), StringComparison.InvariantCultureIgnoreCase);

        return (await GenerateInternal<ExampleTextResponse>(prompt))!.Text;
    }

    public async Task<CreateReadingComprehensionExerciseResponse?> GenerateReadingComprehensionExerciseAsync(CreateReadingComprehensionExerciseRequest request)
    {
        var prompt = _aiLearningPromptsOptions.PromptForGeneratingReadingComprehensionExercise
            .Replace("{words}", string.Join(',', request.Words), StringComparison.InvariantCulture);

        return await GenerateInternal<CreateReadingComprehensionExerciseResponse>(prompt);
    }

    public async Task<CheckReadingComprehensionExerciseResponse?> CheckReadingComprehensionExerciseAsync(CheckReadingComprehensionExerciseRequest request)
    {
        var prompt = _aiLearningPromptsOptions.PromptForCheckingIfReadingComprehensionExercise
            .Replace("{text}", request.Text, StringComparison.InvariantCulture)
            .Replace("{questions}", string.Join(',', request.Questions), StringComparison.InvariantCulture)
            .Replace("{answers}", string.Join(',', request.Answers), StringComparison.InvariantCulture);

        return await GenerateInternal<CheckReadingComprehensionExerciseResponse>(prompt);
    }

    private async Task<T?> GenerateInternal<T>(string prompt)
    {
        var response = await _client.Models.GenerateContentAsync(_geminiOptions.Model, prompt, _defaultGenerateContentConfig);
        if (response.Candidates is {Count: > 0})
        {
            if (response.Candidates[0].Content is {Parts.Count: > 0})
            {
                var json = response.Candidates[0].Content?.Parts?[0].Text;
                return JsonSerializer.Deserialize<T>(json!, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        return default;
    }
}
