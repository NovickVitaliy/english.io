namespace Learning.Features.Decks.Models;

public class CreateDeckRequest
{
    public string DeckTopic { get; set; } = null!;
    public bool IsStrict { get; set; } = false;
}
