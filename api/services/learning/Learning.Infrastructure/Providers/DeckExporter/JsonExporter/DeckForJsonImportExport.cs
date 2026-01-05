using Learning.Domain.Models;

namespace Learning.Infrastructure.Providers.DeckExporter.JsonExporter;

public record DeckForJsonImportExport(string Topic, bool IsStrict, IReadOnlyCollection<WordUnit> WordUnits);
