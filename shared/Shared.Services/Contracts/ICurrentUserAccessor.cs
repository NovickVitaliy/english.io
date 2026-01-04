namespace Shared.Services.Contracts;

public interface ICurrentUserAccessor
{
    int? GetExampleSentencesPerWord();
    int? GetCountOfWordsForPractice();
    string? GetNotificationChannel();
    bool? IsTelegramConnected();
    string? GetPracticeDifficulty();
    string? GetEmail();
    string? GetName();
    string? GetEmailVerified();
    string? GetClaimValue(string claimType);
}
