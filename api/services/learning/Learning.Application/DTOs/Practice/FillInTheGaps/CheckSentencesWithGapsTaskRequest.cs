using Shared.Requests;

namespace Learning.Application.DTOs.Practice.FillInTheGaps;

public record CheckSentencesWithGapsTaskRequest(Guid DeckId, SentenceWithFilledGap[] SentencesWithFilledGaps) : IBaseRequest
{
    public RequestValidationResult IsValid()
    {
        var result = DeckId != Guid.Empty && SentencesWithFilledGaps.Length > 0;
        
        return result
            ? new RequestValidationResult(true)
            : new RequestValidationResult(false, "Invalid request");
    }
}
