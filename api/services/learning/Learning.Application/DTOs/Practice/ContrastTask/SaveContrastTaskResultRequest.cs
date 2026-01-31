using System.Collections.Immutable;

namespace Learning.Application.DTOs.Practice.ContrastTask;

public record SaveContrastTaskResultRequest(Guid DeckId, ImmutableDictionary<Guid, (bool IsCorrect, string Word)> AnswersMap);
