namespace Learning.Features.Settings.Models.Sessions;

public record SessionDto(
    string[] Words,
    double FirstTaskPercentageSuccess,
    double SecondTaskPercentageSuccess,
    double ThirdTaskPercentageSuccess,
    double FourthTaskPercentageSuccess,
    DateTime PracticeDate);
