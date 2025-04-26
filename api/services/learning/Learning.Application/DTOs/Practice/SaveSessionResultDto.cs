namespace Learning.Application.DTOs.Practice;

public record SaveSessionResultDto(
    string[] Words,
    double FirstTaskPercentageSuccess,
    double SecondTaskPercentageSuccess,
    double ThirdTaskPercentageSuccess,
    double FourthTaskPercentageSuccess,
    DateTime PracticeDate);
