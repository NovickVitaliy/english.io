using System.Runtime.CompilerServices;
using Learning.Application.DTOs.Chat;
using Learning.Application.DTOs.Grammar;
using Learning.Application.DTOs.Practice.ContrastTask;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.GetWordsForPractice;
using Learning.Application.DTOs.Practice.ReadingComprehension.Check;
using Learning.Application.DTOs.Practice.ReadingComprehension.Create;
using Learning.Application.DTOs.Practice.TranslateWords;
using Learning.Domain.Models;

namespace Learning.Application.Contracts.Api;

public interface IAiLearningService
{
    const string HttpClientKey = "AiLearningService";
    Task<WordUnit?> GetTranslatedWordWithExamplesAsync(string word, int exampleSentences);
    Task<bool> DoesWordComplyToTheArticle(string word, string topic);
    Task<TranslatedWordResult[]?> VerifyWordsTranslations(CheckTranslateWordsTaskRequest taskRequest);
    Task<SentenceWithGap[]?> GenerateSentencesWithGaps(WordForPractice[] wordsForPractice);
    Task<string> GenerateExampleTextAsync(string[] words);
    Task<CreateReadingComprehensionExerciseResponse?> GenerateReadingComprehensionExerciseAsync(WordForPractice[] wordsForPractice);
    Task<CheckReadingComprehensionExerciseResponse?> CheckReadingComprehensionExerciseAsync(CheckReadingComprehensionExerciseRequest request);
    Task<SentenceWithFilledGapResult[]?> CheckSentencesWithGapsTaskAsync(SentenceWithFilledGap[] requestSentencesWithFilledGaps);
    Task<ContrastTaskUnit[]?> GenerateContrastTaskForWordsAsync(WordForPractice[] wordsForPractice);
    IAsyncEnumerable<string> GenerateChatResponseAsync(List<ChatMessageDto> messages, CancellationToken ct = default);
    Task<AnalyzeTextResponse?> AnalyzeTextAsync(AnalyzeTextRequest request);
}
