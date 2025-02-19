using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.Decks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Controllers;

[ApiController]
[Route("api/decks")]
[Authorize]
public class DecksController : ControllerBase
{
    private readonly IDecksService _decksService;

    public DecksController(IDecksService decksService)
    {
        _decksService = decksService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDeckAsync(CreateDeckRequest request)
    {
        return (await _decksService.CreateDeckAsync(request)).ToApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetDecksForUserAsync([FromQuery] GetDecksForUserRequest request)
    {
        return (await _decksService.GetDecksForUser(request)).ToApiResponse();
    }

    [HttpGet("{deckId:guid}")]
    public async Task<IActionResult> GetDeckAsync(Guid deckId)
    {
        return (await _decksService.GetDeckAsync(deckId)).ToApiResponse();
    }

    [HttpPost("{deckId:guid}")]
    public async Task<IActionResult> CreateDeckWordAsync(Guid deckId, CreateDeckWordRequest request)
    {
        request = request with
        {
            DeckId = deckId
        };
        return (await _decksService.CreateDeckWordAsync(request)).ToApiResponse();
    }
}
