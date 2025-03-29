namespace Learning.Features.Practice.Models.TranslateWords;

public class TranslatedWord
{
    public string OriginalWord { get; set; } = null!;
    public string Translated { get; set; } = null!;
    public string OriginalLanguage { get; set; } = null!;
    public string TranslatedLanguage { get; set; } = null!;
}
