namespace Learning.Application.DTOs.Grammar;

public record AnalyzeTextResponse(
    string OriginalText,
    TextIssue[] Issues);