namespace Shared.MessageBus.Events;

public record UserCreatedPreferences(
    string UserEmail,
    int ExampleSentences,
    int CountOfWordsForPractice,
    string NotificationChannel,
    bool IsTelegramConnected,
    string PracticeDifficulty) : BaseEvent;
