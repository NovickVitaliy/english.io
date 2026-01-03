using CsvHelper.Configuration;

namespace Learning.Infrastructure.CsvMappers;

public sealed class WordSenseCsvMap : ClassMap<WordForExport>
{
    public WordSenseCsvMap()
    {
        Map(x => x.Word).Name("Word");
        Map(x => x.PartOfSpeech).Name("PartOfSpeech");
        Map(x => x.Definition).Name("Definition");
        Map(x => x.UkrainianTranslation).Name("UkrainianTranslation");
        Map(x => x.UsageLabel).Name("UsageLabel");
        Map(x => x.ExampleSentences).Name("ExampleSentences");
    }
}
