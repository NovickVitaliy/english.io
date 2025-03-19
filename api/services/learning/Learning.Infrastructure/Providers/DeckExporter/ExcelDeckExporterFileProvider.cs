using Learning.Application.Contracts.Providers;
using Learning.Application.DTOs.Decks;
using Learning.Domain.Models;
using OfficeOpenXml;

namespace Learning.Infrastructure.Providers.DeckExporter;

public class ExcelDeckExporterFileProvider : IDeckExporterFileProvider
{
    private readonly ExportDeckFileType _handlesFileType = ExportDeckFileType.Excel;

    public bool Handles(ExportDeckFileType exportDeckFileType) => exportDeckFileType == _handlesFileType;

    public async Task<Stream> ExportDeckAsync(Deck deck)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var stream = new MemoryStream();
        try
        {
            using var excelPackage = new ExcelPackage(stream);

            var worksheet = excelPackage.Workbook.Worksheets.Add("Deck");

            worksheet.Cells[1, 1].Value = "English Version";
            worksheet.Cells[1, 2].Value = "Ukrainian Version";
            worksheet.Cells[1, 3].Value = "Example Sentences";

            int row = 2;
            foreach (var word in deck.DeckWords)
            {
                worksheet.Cells[row, 1].Value = word.EnglishVersion;
                worksheet.Cells[row, 2].Value = word.UkrainianVersion;
                worksheet.Cells[row, 3].Value = string.Join(";", word.ExampleSentences);
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
