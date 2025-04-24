using System.Text.Json.Serialization;

namespace Learning.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationChannel
{
    Email,
    Telegram
}
