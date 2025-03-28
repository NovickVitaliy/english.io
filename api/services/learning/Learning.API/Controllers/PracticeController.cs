using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice;
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
}
