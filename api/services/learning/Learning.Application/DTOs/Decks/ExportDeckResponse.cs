namespace Learning.Application.DTOs.Decks;

public record ExportDeckResponse(
    Stream FileStream,
    string ContentType,
    string FileName);
