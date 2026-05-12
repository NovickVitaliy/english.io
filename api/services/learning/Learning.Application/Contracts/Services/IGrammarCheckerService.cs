using Learning.Application.DTOs.Grammar;

namespace Learning.Application.Contracts.Services;

public interface IGrammarCheckerService
{
    Task<AnalyzeTextResponse> AnalyzeAsync(string text);
}
