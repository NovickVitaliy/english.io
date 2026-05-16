namespace Learning.Domain.Models;

public class WordProgressHistory
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public Guid SenseId { get; set; }
    public Guid DeckId { get; set; }
    public PracticeTask Task { get; set; }
    public bool WasCorrect { get; set; }
    public DateTime PracticedAt { get; set; }
}