namespace Learning.Features.Practice.Models.FillInTheGaps;

public class FillInTheGapsData
{
    public string[] Words { get; init; }
    public Dictionary<string, string> FillInTheGapsResult { get; }

    public FillInTheGapsData(int wordsCount, string[] words)
    {
        Words = new string[wordsCount];
        FillInTheGapsResult = words.ToDictionary(x => x, _ => "");
    }
}
