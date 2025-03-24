using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Practice;
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

    [HttpPost("translate-words-from-english-to-ukrainian")]
    public async Task<IActionResult> TranslateWordsFromEnglishToUkrainianTask(TranslateWordsRequest request)
    {
        return (await _practiceService.TranslateWords(request)).ToApiResponse();
    }
}
