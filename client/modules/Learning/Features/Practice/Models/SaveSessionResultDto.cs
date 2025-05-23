namespace Learning.Features.Practice.Models;

public record SaveSessionResultDto(
    string[] Words,
    double FirstTaskPercentageSuccess,
    double SecondTaskPercentageSuccess,
    double ThirdTaskPercentageSuccess,
    double FourthTaskPercentageSuccess,
    DateTime PracticeDate);
