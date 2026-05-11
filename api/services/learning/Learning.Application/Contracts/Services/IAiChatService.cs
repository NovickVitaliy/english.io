using Learning.Application.DTOs.Chat;

namespace Learning.Application.Contracts.Services;

public interface IAiChatService
{
    IAsyncEnumerable<string> StreamAsync(List<ChatMessageDto> messages, CancellationToken ct = default);
}
