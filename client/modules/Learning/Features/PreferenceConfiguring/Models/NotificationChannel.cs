using System.Text.Json.Serialization;

namespace Learning.Features.PreferenceConfiguring.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationChannel
{
    Email,
    Telegram
}
