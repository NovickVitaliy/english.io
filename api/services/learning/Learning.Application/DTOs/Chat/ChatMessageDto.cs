namespace Learning.Application.DTOs.Chat;

public class ChatMessageDto
{
    public string Role { get; set; } = string.Empty; // "user" | "assistant"
    public string Content { get; set; } = string.Empty;
}
