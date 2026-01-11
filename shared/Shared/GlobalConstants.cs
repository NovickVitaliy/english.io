namespace Shared;

public static class GlobalConstants
{
    public static class ApplicationClaimTypes
    {
        public const string PreferencesConfigured = "PreferencesConfigured";
        public const string ExampleSentencesPerWord = "ExampleSentencesPerWord";
        public const string CountOfWordsForPractice = "CountOfWordsForPractice";
        public const string NotificationChannel = "NotificationChannel";
        public const string IsTelegramConnected = "IsTelegramConnected";
        public const string PracticeDifficulty = "PracticeDifficulty";
    }

    public static class Languages
    {
        public const string EnglishCode = "en-US";
        public const string UkrainianCode = "uk-UA";

        public const string English = "english";
        public const string Ukrainian = "ukrainian";

        public static readonly string[] SupportedLanguages = ["ukrainian", "english"];
    }
}
