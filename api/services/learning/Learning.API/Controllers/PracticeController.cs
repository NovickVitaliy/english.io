using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice;
using Learning.Application.DTOs.Practice.ReadingComprehension.Check;
using Learning.Application.DTOs.Practice.ReadingComprehension.Create;
using Learning.Application.DTOs.Practice.TranslateWords;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Controllers;

[ApiController]
[Route("api/practice")]
public class PracticeController : ControllerBase
{
    private readonly IPracticeService _practiceService;

    public PracticeController(IPracticeService practiceService)
    {
        _practiceService = practiceService;
    }

    [HttpPost("translate-words")]
    public async Task<IActionResult> TranslateWordsTask(TranslateWordsRequest request)
    {
        return (await _practiceService.TranslateWords(request)).ToApiResponse();
    }

    [HttpGet("sentences-with-gaps")]
    public async Task<IActionResult> GetSentencesWithGaps([FromQuery] string[] words)
    {
        return (await _practiceService.GetSentencesWithGapsAsync(words)).ToApiResponse();
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

    [HttpGet("reading-comprehension")]
    public async Task<IActionResult> GetReadingComprehensionExercise([FromQuery] CreateReadingComprehensionExerciseRequest request)
    {
        return (await _practiceService.CreateReadingComprehensionExerciseAsync(request)).ToApiResponse();
    }

    [HttpPost("reading-comprehension-check")]
    public async Task<IActionResult> CheckReadingComprehensionExercise([FromQuery] CheckReadingComprehensionExerciseRequest request)
    {
        return (await _practiceService.CheckReadingComprehensionExerciseAsync(request)).ToApiResponse();
    }
}
