using Shared.Requests;

namespace Learning.Application.DTOs.Practice.ReadingComprehension.Check;

public record CheckReadingComprehensionExerciseRequest(
    string Text,
    string[] Questions,
    string[] Answers) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        return !string.IsNullOrWhiteSpace(Text) && Questions.Length > 0 && Answers.Length > 0
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false);
    }
}
