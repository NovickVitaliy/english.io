using System.Runtime.CompilerServices;
using Learning.Application.Contracts.Api;
using Learning.Application.DTOs.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class AiChatController : ControllerBase
{
    private readonly IAiLearningService _aiLearningService;

    public AiChatController(IAiLearningService aiLearningService)
    {
        _aiLearningService = aiLearningService;
    }

    [HttpPost]
    public async Task Stream(
        [FromBody] ChatRequest request,
        CancellationToken cancellationToken)
    {
        Response.Headers.ContentType = "text/plain";

        await foreach (var chunk in _aiLearningService.GenerateChatResponseAsync(request.Messages, cancellationToken))
        {
            await Response.WriteAsync(chunk, cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }
}
