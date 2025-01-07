using Learning.Features.PreferenceConfiguring.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Learning.Features.PreferenceConfiguring.Components;

public partial class ConfigurePreferenceForm : ComponentBase
{
    private ConfigurePreferenceRequest _request = new ConfigurePreferenceRequest();
    private MudForm? _form = null!;

    private void HandleDailySessionsNumberChange(int sessionsCount)
    {
        _request.DailySessionsCount = sessionsCount;
        if (sessionsCount > _request.DailySessionsReminderTimes.Count)
        {
            _request.DailySessionsReminderTimes.AddRange(Enumerable.Range(0, sessionsCount).Select(_ => TimeSpan.Zero));
        }
        else
        {
            if (sessionsCount < _request.DailySessionsReminderTimes.Count)
            {
                _request.DailySessionsReminderTimes.RemoveRange(sessionsCount, _request.DailySessionsCount - sessionsCount);
            }
        }
    }
    private async Task Submit()
    {
        Console.WriteLine(_request);
        foreach (var time in _request.DailySessionsReminderTimes)
        {
            Console.WriteLine(time);
        }
    }
}