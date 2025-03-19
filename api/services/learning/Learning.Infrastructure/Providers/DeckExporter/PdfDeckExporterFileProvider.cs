using Learning.Application.Contracts.Providers;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Learning.Infrastructure.Providers.DeckExporter;

public class PdfDeckExporterFileProvider : IDeckExporterFileProvider
{
    private readonly ExportDeckFileType _exportDeckFileType = ExportDeckFileType.Pdf;

    public bool Handles(ExportDeckFileType exportDeckFileType) => exportDeckFileType == _exportDeckFileType;

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

                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                        {
                            foreach (var word in deck.DeckWords)
                            {
                                column.Item().Text($"English Version: {word.EnglishVersion}");
                                column.Item().Text($"Ukrainian Version: {word.UkrainianVersion}");
                                column.Item().Text($"Example sentences:\n {string.Join("\n", word.ExampleSentences)}").Italic().FontSize(10);
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
