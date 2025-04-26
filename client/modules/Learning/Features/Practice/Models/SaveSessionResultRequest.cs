namespace Learning.Features.Practice.Models;

public record SaveSessionResultRequest(
    string[] Words,
    double FirstTaskPercentageSuccess,
    double SecondTaskPercentageSuccess,
    double ThirdTaskPercentageSuccess,
    double FourthTaskPercentageSuccess);
