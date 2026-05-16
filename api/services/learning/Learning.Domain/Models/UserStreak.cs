namespace Learning.Domain.Models;

public class UserStreak
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateTime LastPracticeDate { get; set; }
}