namespace Learning.Features.Practice.Models.FillInTheGaps;

public class FillInTheGapsData
{
    public string[] Words { get; init; }
    public Dictionary<string, string> FillInTheGapsResult { get; }

    public FillInTheGapsData(int wordsCount, string[] words)
    {
        Words = new string[wordsCount];
        // distinct is used because sometimes ai translates two synonyms to the same word and it may cause problems
        FillInTheGapsResult = words.Distinct().ToDictionary(x => x, _ => "");
    }
}
