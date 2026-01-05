using System.Net.Mime;

namespace Learning.Application.DTOs.Decks;

public enum ExportDeckFileType
{
    Csv,
    Excel,
    Pdf,
    Json
}

public static class ExportDeckFileTypeHelper
{
    public static string GetHttpContentType(ExportDeckFileType exportDeckFileType) =>
        exportDeckFileType switch
        {
            ExportDeckFileType.Csv => MediaTypeNames.Text.Csv,
            ExportDeckFileType.Excel => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ExportDeckFileType.Pdf => MediaTypeNames.Application.Pdf,
            ExportDeckFileType.Json => MediaTypeNames.Application.Json,
            _ => throw new ArgumentOutOfRangeException(nameof(exportDeckFileType), exportDeckFileType, null)
        };

    public static object GetFileExtension(ExportDeckFileType requestType) =>
        requestType switch
        {
            ExportDeckFileType.Csv => ".csv",
            ExportDeckFileType.Excel => ".xlsx",
            ExportDeckFileType.Pdf => ".pdf",
            ExportDeckFileType.Json => ".json",
            _ => throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null)
        };
}
