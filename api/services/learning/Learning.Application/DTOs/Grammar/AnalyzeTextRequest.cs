namespace Learning.Application.DTOs.Grammar;

public record AnalyzeTextRequest(string Text);

public static class PromptBuilder
{
    public static string BuildGrammarPrompt(string text)
    {
        return $$"""
                 You are an English writing correction assistant.

                 Analyze the following text and detect:
                 - grammar mistakes
                 - spelling mistakes
                 - punctuation mistakes
                 - awkward phrasing
                 - unnatural wording

                 Return ONLY structured JSON in this exact schema:

                 {
                   "issues": [
                     {
                       "original": "string",
                       "suggested": "string",
                       "explanation": "string",
                       "category": "grammar|spelling|punctuation|style",
                       "startIndex": 0,
                       "length": 0
                     }
                   ]
                 }

                 RULES:
                 - startIndex must point to the first character of the mistake in the original text
                 - length must equal the mistake length
                 - explanation must be short and learner-friendly
                 - do not rewrite the entire text
                 - only return actual issues
                 - output JSON ONLY

                 TEXT:
                 {{text}}
                 """;
    }
}
