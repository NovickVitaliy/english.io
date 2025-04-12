using System.ComponentModel.DataAnnotations.Schema;

namespace English.IO.Bot.Models;

public class User
{
    public int Id { get; init; }
    public long TelegramChatId { get; init; }
    public bool HasSubmittedCode { get; set; }

    [Column(TypeName = "varchar(50)")]
    public UserState UserState { get; set; } = UserState.None;
}
