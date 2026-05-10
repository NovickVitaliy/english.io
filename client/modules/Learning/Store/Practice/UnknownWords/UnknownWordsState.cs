using Fluxor;

namespace Learning.Store.Practice.UnknownWords;

[FeatureState]
public record UnknownWordsState(UnknownWord[] UnknownWords, bool IsSaving)
{
    private UnknownWordsState() : this([], false)
    {

    }
}

public class UnknownWord
{
    public string Word { get; init; }
    public bool IsSaved { get; set; }

    public UnknownWord(string word)
    {
        Word = word;
    }
}
