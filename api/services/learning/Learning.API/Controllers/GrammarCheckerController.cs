using Learning.Application.Contracts.Api;
using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Grammar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Controllers;

[Authorize]
[ApiController]
[Route("api/grammar")]
public class GrammarCheckerController : ControllerBase
{
    private readonly IAiLearningService _aiLearningService;

    public GrammarCheckerController(IAiLearningService aiLearningService)
    {
        _aiLearningService = aiLearningService;
    }

    [HttpPost]
    public async Task<IActionResult> CheckGrammarAsync(AnalyzeTextRequest request)
    {
        return Ok(await _aiLearningService.AnalyzeTextAsync(request));
    }
}
