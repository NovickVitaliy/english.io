using System.Text.Json.Serialization;

namespace Learning.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PracticeDifficulty
{
    VeryEasy,
    Easy,
    Medium,
    Hard,
    VeryHard
}
