using System.Text.Json;
using Google.GenAI;
using Google.GenAI.Types;
using Learning.Application.Contracts.Api;
using Learning.Application.DTOs.Decks;
using Learning.Application.DTOs.Practice.ContrastTask;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.GetWordsForPractice;
using Learning.Application.DTOs.Practice.ReadingComprehension.Check;
using Learning.Application.DTOs.Practice.ReadingComprehension.Create;
using Learning.Application.DTOs.Practice.TranslateWords;
using Learning.Domain.Models;
using Learning.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Polly.Registry;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Learning.Infrastructure.Api;

public class GeminiAiLearningService : IAiLearningService
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        WriteIndented = false
    };

    private readonly Client _client;
    private readonly GeminiOptions _geminiOptions;
    private readonly AiLearningPromptsOptions _aiLearningPromptsOptions;
    private readonly GenerateContentConfig _defaultGenerateContentConfig;
    private readonly ResiliencePipelineProvider<string> _resiliencePipelineProvider;

    public GeminiAiLearningService(
        Client client,
        IOptions<GeminiOptions> geminiOptions,
        IOptions<AiLearningPromptsOptions> aiLearningPromptsOptions,
        ResiliencePipelineProvider<string> resiliencePipelineProvider)
    {
        _client = client;
        _resiliencePipelineProvider = resiliencePipelineProvider;
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

    public async Task<TranslatedWordResult[]?> VerifyWordsTranslations(CheckTranslateWordsTaskRequest taskRequest)
    {
        var prompt = _aiLearningPromptsOptions.PromptForCheckingIfTranslationsAreCorrect
            .Replace("{OriginalLanguage}", taskRequest.OriginalLanguage, StringComparison.InvariantCulture)
            .Replace("{TranslatedLanguage}", taskRequest.TranslateLanguage, StringComparison.InvariantCulture)
            .Replace("{TranslatedWordsJson}", JsonSerializer.Serialize(taskRequest.TranslatedWords, Options), StringComparison.InvariantCulture);

        return (await GenerateInternal<TranslatedWordResult[]>(prompt));
    }

    public async Task<SentenceWithGap[]?> GenerateSentencesWithGaps(WordForPractice[] wordsForPractice)
    {
        var prompt = _aiLearningPromptsOptions.PromptForGeneratingSentencesWithGaps
            .Replace("{WordForPracticeJson}", JsonSerializer.Serialize(wordsForPractice, Options), StringComparison.InvariantCulture);

        return await GenerateInternal<SentenceWithGap[]>(prompt);
    }

    public async Task<string> GenerateExampleTextAsync(string[] words)
    {
        var prompt = _aiLearningPromptsOptions.PromptForGeneratingExampleText
            .Replace("{words}", string.Join(',', words), StringComparison.InvariantCultureIgnoreCase);

        return (await GenerateInternal<ExampleTextResponse>(prompt))!.Text;
    }

    public async Task<CreateReadingComprehensionExerciseResponse?> GenerateReadingComprehensionExerciseAsync(WordForPractice[] wordsForPractice)
    {
        var prompt = _aiLearningPromptsOptions.PromptForGeneratingReadingComprehensionExercise
            .Replace("{WordsForPracticeJson}", JsonSerializer.Serialize(wordsForPractice, Options), StringComparison.InvariantCulture);

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

    public async Task<SentenceWithFilledGapResult[]?> CheckSentencesWithGapsTaskAsync(SentenceWithFilledGap[] requestSentencesWithFilledGaps)
    {
        var prompt = _aiLearningPromptsOptions.PromptForCheckingSentencesWithGapsTask
            .Replace("{SentenceWithFilledGapJson}", JsonSerializer.Serialize(requestSentencesWithFilledGaps, Options), StringComparison.InvariantCulture);

        return await GenerateInternal<SentenceWithFilledGapResult[]>(prompt);
    }

    public async Task<ContrastTaskUnit[]?> GenerateContrastTaskForWordsAsync(WordForPractice[] wordsForPractice)
    {
        var prompt = _aiLearningPromptsOptions.PromptForGeneratingSentencesWithGaps
            .Replace("{WordForPracticeJson}", JsonSerializer.Serialize(wordsForPractice, Options), StringComparison.InvariantCulture);

        return await GenerateInternal<ContrastTaskUnit[]>(prompt);
    }

    private async Task<T?> GenerateInternal<T>(string prompt, CancellationToken cancellationToken = default)
    {
        var pipeline = _resiliencePipelineProvider.GetPipeline("gemini-api-pipeline");

        return await pipeline.ExecuteAsync(async token =>
        {
            var response = await _client.Models.GenerateContentAsync(
                _geminiOptions.Model,
                prompt,
                _defaultGenerateContentConfig);

            var json = response.Candidates?
                .FirstOrDefault()?
                .Content?
                .Parts?
                .FirstOrDefault()?
                .Text;

            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("Gemini returned empty content");

            try
            {
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new JsonException("Deserialized null result");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Invalid JSON returned by Gemini", ex);
            }

        }, cancellationToken);
    }
}
