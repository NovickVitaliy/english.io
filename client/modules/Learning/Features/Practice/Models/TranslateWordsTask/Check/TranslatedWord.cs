namespace Learning.Features.Practice.Models.TranslateWordsTask.Check;

public class TranslatedWord
{
    public string OriginalWord { get; set; } = null!;
    public string Definition { get; set; } = null!;
    public string Translated { get; set; } = null!;
    public Guid SenseId { get; set; }
}
