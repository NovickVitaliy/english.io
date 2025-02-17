namespace Learning.Domain.Models;

public class Deck
{
    public Guid Id { get; init; }
    public string UserEmail { get; init; } = null!;
    public string Topic { get; set; } = null!;
    public bool IsStrict { get; set; }
    public List<DeckWord> DeckWords { get; set; } = [];
}
