namespace Learning.Features.Practice.Models.FillInTheGaps;

public class FillInTheGapsData
{
    public string[] Words { get; init; }

    public FillInTheGapsData(int wordsCount)
    {
        Words = new string[wordsCount];
    }
}
