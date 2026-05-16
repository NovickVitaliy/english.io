using Learning.Infrastructure.Services.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services.Contracts;

namespace Learning.Controllers;

[ApiController]
[Route("api/statistics")]
[Authorize]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public StatisticsController(IStatisticsService statisticsService, ICurrentUserAccessor currentUserAccessor)
    {
        _statisticsService = statisticsService;
        _currentUserAccessor = currentUserAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatistics([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var userEmail = _currentUserAccessor.GetEmail()!;
        var dateFrom = DateTime.SpecifyKind(from ?? DateTime.UtcNow.AddDays(-30), DateTimeKind.Utc);
        var dateTo = DateTime.SpecifyKind(to ?? DateTime.UtcNow, DateTimeKind.Utc);

        var result = await _statisticsService.GetUserStatisticsAsync(userEmail, dateFrom, dateTo);
        return result.Success ? Ok(result.Data) : BadRequest(result.Description);
    }

    [HttpGet("word-progress")]
    public async Task<IActionResult> GetWordProgress([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var userEmail = _currentUserAccessor.GetEmail()!;
        var dateFrom = DateTime.SpecifyKind(from ?? DateTime.UtcNow.AddDays(-30), DateTimeKind.Utc);
        var dateTo = DateTime.SpecifyKind(to ?? DateTime.UtcNow, DateTimeKind.Utc);

        var result = await _statisticsService.GetWordProgressHistoryAsync(userEmail, dateFrom, dateTo);
        return result.Success ? Ok(result.Data) : BadRequest(result.Description);
    }
}
