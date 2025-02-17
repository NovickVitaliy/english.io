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

    [HttpGet("{email:alpha}")]
    public async Task<IActionResult> GetDecksForUserAsync(GetDecksForUserRequest request)
    {
        return (await _decksService.GetDecksForUser(request)).ToApiResponse();
    }
}
