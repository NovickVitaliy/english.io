using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.ContrastTask;
using Learning.Application.DTOs.Practice.FillInTheGaps;
using Learning.Application.DTOs.Practice.GetTranslationTask;
using Learning.Application.DTOs.Practice.GetWordsForPractice;
using Learning.Application.DTOs.Practice.ReadingComprehension.Check;
using Learning.Application.DTOs.Practice.Sessions;
using Learning.Application.DTOs.Practice.TranslateWords;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Controllers;

[ApiController]
[Authorize]
[Route("api/practice")]
public class PracticeController : ControllerBase
{
    private readonly IPracticeService _practiceService;

    public PracticeController(IPracticeService practiceService)
    {
        _practiceService = practiceService;
    }

    [HttpGet("{deckId:guid}/words")]
    public async Task<IActionResult> GetWordsForPracticeAsync(Guid deckId)
    {
        return (await _practiceService.GetWordsForPracticeAsync(deckId)).ToApiResponse();
    }

    [HttpPost("{deckId:guid}/get-translation-task")]
    public async Task<IActionResult> GetTranslationTaskAsync(Guid deckId, GetTranlationTaskRequest request)
    {
        return (await _practiceService.GetWordForTranslationTaskAsync(deckId, request)).ToApiResponse();
    }

    [HttpPost("check-translation-task")]
    public async Task<IActionResult> CheckTranslateWordsTaskAsync(CheckTranslateWordsTaskRequest taskRequest)
    {
        return (await _practiceService.CheckTranslateWordsTaskAsync(taskRequest)).ToApiResponse();
    }

    [HttpPost("{deckId:guid}/get-sentences-with-gaps-task")]
    public async Task<IActionResult> GetSentencesWithGaps(Guid deckId, WordForPractice[] wordsForPractice)
    {
        return (await _practiceService.GetSentencesWithGapsAsync(deckId, wordsForPractice)).ToApiResponse();
    }

    [HttpPost("check-sentences-with-gaps-task")]
    public async Task<IActionResult> CheckSentencesWithGapsTask(CheckSentencesWithGapsTaskRequest request)
    {
        return (await _practiceService.CheckSentencesWithGapsTask(request)).ToApiResponse();
    }

    [HttpPost("{deckId:guid}/get-contrast-task")]
    public async Task<IActionResult> GetContrastTaskAsync(Guid deckId, WordForPractice[] wordsForpractice)
    {
        return (await _practiceService.GetContrastTaskAsync(deckId, wordsForpractice)).ToApiResponse();
    }

    [HttpPost("save-contrast-task")]
    public async Task<IActionResult> SaveContrastTaskResultAsync(SaveContrastTaskResultRequest request)
    {
        return (await _practiceService.SaveContrastTaskResultAsync(request)).ToApiResponse();
    }

    [HttpGet("example-text")]
    public async Task<IActionResult> GetExampleText([FromQuery] string[] words)
    {
        return (await _practiceService.GetExampleTextAsync(words)).ToApiResponse();
    }

    [HttpPost("save-session-result")]
    public async Task<IActionResult> SaveSessionResultAsync(SaveSessionResultRequest request)
    {
        return (await _practiceService.SaveSessionResultAsync(request)).ToApiResponse();
    }

    [HttpGet("get-reading-comprehension")]
    public async Task<IActionResult> GetReadingComprehensionExercise(WordForPractice[] wordsForPractice)
    {
        return (await _practiceService.CreateReadingComprehensionExerciseAsync(wordsForPractice)).ToApiResponse();
    }

    [HttpPost("reading-comprehension-check")]
    public async Task<IActionResult> CheckReadingComprehensionExercise(CheckReadingComprehensionExerciseRequest request)
    {
        return (await _practiceService.CheckReadingComprehensionExerciseAsync(request)).ToApiResponse();
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessionsForUser([FromQuery] GetSessionsForUserRequest request)
    {
        return (await _practiceService.GetSessionsForUserAsync(request)).ToApiResponse();
    }
}
