using Learning.Application.Contracts.Providers;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Learning.Infrastructure.Providers.DeckExporter;

public class PdfDeckExporterFileProvider : BaseDeckExporterProvider, IDeckExporterFileProvider
{
    private const ExportDeckFileType ExportDeckFileType = Application.DTOs.Decks.ExportDeckFileType.Pdf;

    public PdfDeckExporterFileProvider(LearningDbContext learningDbContext) : base(learningDbContext)
    { }

    public bool Handles(ExportDeckFileType exportDeckFileType) => exportDeckFileType == ExportDeckFileType;

    public async Task<Stream> ExportDeckAsync(Deck deck)
    {
        var stream = new MemoryStream();

        try
        {
            Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header().Text($"Deck: {deck.Topic}").FontSize(20).Bold();

                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(async column =>
                        {
                            foreach (var word in FlattenToWordSenses(await LoadWordsFromDatabase(deck)))
                            {
                                column.Item().Text($"Word : {word.Word}");
                                column.Item().Text($"Part Of Speech: {word.PartOfSpeech}");
                                column.Item().Text($"Definition: {word.Definition}");
                                column.Item().Text($"Ukrainian Translation: {word.UkrainianTranslation}");
                                column.Item().Text($"Usage Label: {word.UsageLabel}");
                                column.Item().Text($"Example sentences:\n {word.ExampleSentences}").Italic().FontSize(10);
                                column.Item().PaddingBottom(5);
                            }
                        });

                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                    });
                })
                .GeneratePdf(stream);

            stream.Position = 0;
            return stream;
        }
        catch (Exception)
        {
            await stream.DisposeAsync();
            throw;
        }
    }
}
