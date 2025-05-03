namespace Learning.Application.DTOs.Practice.Sessions;

public record SessionDto(
    string[] Words,
    double FirstTaskPercentageSuccess,
    double SecondTaskPercentageSuccess,
    double ThirdTaskPercentageSuccess,
    double FourthTaskPercentageSuccess,
    DateTime PracticeDate);
