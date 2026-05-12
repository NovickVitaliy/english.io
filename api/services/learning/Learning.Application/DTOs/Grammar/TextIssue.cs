namespace Learning.Application.DTOs.Grammar;

public record TextIssue(
    Guid Id,
    string Original,
    string Suggested,
    string Explanation,
    string Category,
    int StartIndex,
    int Length);