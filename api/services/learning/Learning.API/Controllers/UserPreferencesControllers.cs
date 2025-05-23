using Learning.Application.Contracts.Services;
using Learning.Application.DTOs.UserPreferences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Controllers;

[Route("api/user-preferences")]
[ApiController]
[Authorize]
public class UserPreferencesControllers : ControllerBase
{
    private readonly IUserPreferencesService _userPreferencesService;

    public UserPreferencesControllers(IUserPreferencesService userPreferencesService)
    {
        _userPreferencesService = userPreferencesService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateUserPreferencesRequest request)
    {
        return (await _userPreferencesService.CreateUserPreferencesAsync(request)).ToApiResponse();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] GetUserPreferencesRequest request)
    {
        return (await _userPreferencesService.GetUserPreferencesAsync(request)).ToApiResponse();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserPreferencesRequest request)
    {
        request = request with
        {
            Id = id
        };

        return (await _userPreferencesService.UpdateUserPreferencesAsync(request)).ToApiResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] DeleteUserPreferencesRequest request)
    {
        return (await _userPreferencesService.DeleteUserPreferencesAsync(request)).ToApiResponse();
    }
}