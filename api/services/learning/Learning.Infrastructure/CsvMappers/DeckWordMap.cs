using CsvHelper.Configuration;
using Learning.Domain.Models;

namespace Learning.Infrastructure.CsvMappers;

public sealed class DeckWordMap : ClassMap<DeckWord>
{
    public DeckWordMap()
    {
        Map(x => x.EnglishVersion);
        Map(x => x.UkrainianVersion);
        Map(x => x.ExampleSentences).Convert(args =>
        {
            var examples = args.Value.ExampleSentences;
            return string.Join(";", examples);
        });
    }
}
