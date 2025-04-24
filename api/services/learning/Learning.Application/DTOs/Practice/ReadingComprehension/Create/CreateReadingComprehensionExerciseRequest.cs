using Shared.Requests;

namespace Learning.Application.DTOs.Practice.ReadingComprehension.Create;

public record CreateReadingComprehensionExerciseRequest(string[] Words): IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        return Words.Length > 0
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false);
    }
};
