namespace Learning.Domain.Models;

public class DeckEntry
{
    public Guid Id { get; init; }
    public Guid WordUnitId { get; init; }
    public Guid WordSenseId { get; init; }
    public float ProgressScore { get; set; }
    public DateTimeOffset? LastTimePracticed { get; set; }
}
