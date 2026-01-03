using Learning.Application.Contracts.Providers;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using Learning.Infrastructure.Database;
using OfficeOpenXml;

namespace Learning.Infrastructure.Providers.DeckExporter;

public class ExcelDeckExporterFileProvider : BaseDeckExporterProvider, IDeckExporterFileProvider
{
    private const ExportDeckFileType HandlesFileType = ExportDeckFileType.Excel;

    public ExcelDeckExporterFileProvider(LearningDbContext learningDbContext) : base(learningDbContext)
    { }

    public bool Handles(ExportDeckFileType exportDeckFileType) => exportDeckFileType == HandlesFileType;

    public async Task<Stream> ExportDeckAsync(Deck deck)
    {
        var stream = new MemoryStream();
        try
        {
            using var excelPackage = new ExcelPackage(stream);

            var worksheet = excelPackage.Workbook.Worksheets.Add("Deck");

            worksheet.Cells[1, 1].Value = "Word";
            worksheet.Cells[1, 2].Value = "Part Of Speech";
            worksheet.Cells[1, 3].Value = "Definition";
            worksheet.Cells[1, 4].Value = "Ukrainian Translation";
            worksheet.Cells[1, 5].Value = "Usage Label";
            worksheet.Cells[1, 6].Value = "Example Sentences";

            int row = 2;
            foreach (var word in FlattenToWordSenses(await LoadWordsFromDatabase(deck)))
            {
                worksheet.Cells[row, 1].Value = word.Word;
                worksheet.Cells[row, 2].Value = word.PartOfSpeech;
                worksheet.Cells[row, 3].Value = word.Definition;
                worksheet.Cells[row, 4].Value = word.UkrainianTranslation;
                worksheet.Cells[row, 5].Value = word.UsageLabel;
                worksheet.Cells[row, 6].Value = word.ExampleSentences;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            await excelPackage.SaveAsync();

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
