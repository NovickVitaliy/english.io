namespace Learning.Domain.Models;

public class SessionResult
{
    public Guid Id { get; init; }
    public string UserEmail { get; init; } = null!;
    public string[] Words { get; init; } = [];
    public double FirstTaskPercentageSuccess { get; init; }
    public double SecondTaskPercentageSuccess { get; init; }
    public double ThirdTaskPercentageSuccess { get; init; }
    public double FourthTaskPercentageSuccess { get; init; }
    public DateTime PracticeDate { get; init; }
}
