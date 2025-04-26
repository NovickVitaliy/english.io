using Shared.Requests;

namespace Learning.Application.DTOs.Practice.ReadingComprehension.Create;

public record CreateReadingComprehensionExerciseResponse(string Text, string[] Questions);
