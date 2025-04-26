using Learning.Application.Validation.Practice;
using Shared.Requests;

namespace Learning.Application.DTOs.Practice;

public record SaveSessionResultRequest(
    string[] Words,
    double FirstTaskPercentageSuccess,
    double SecondTaskPercentageSuccess,
    double ThirdTaskPercentageSuccess,
    double FourthTaskPercentageSuccess) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var validationResult = new SaveSessionResultValidator().Validate(this);

        return validationResult.IsValid
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, validationResult.Errors[0].ErrorMessage);
    }
}
