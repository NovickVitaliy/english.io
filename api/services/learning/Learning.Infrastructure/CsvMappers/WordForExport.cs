namespace Learning.Infrastructure.CsvMappers;

public record WordForExport(
    string Word,
    string PartOfSpeech,
    string Definition,
    string UkrainianTranslation,
    string UsageLabel,
    string ExampleSentences,
    string Synonyms);
